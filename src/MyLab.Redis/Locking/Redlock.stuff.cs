using MyLab.Redis.Abstractions;
using StackExchange.Redis;

namespace MyLab.Redis.Locking;

partial class Redlock
{
    const string UnlockScript =
        """
        if redis.call('get', KEYS[1]) == ARGV[1] then
            return redis.call('del', KEYS[1])
        else
            return 0
        end
        """;

    const string ProlongScript =
        """
        if redis.call('get', KEYS[1]) == ARGV[1] then
            return redis.call('pexpire', KEYS[1], ARGV[2])
        else
            return 0
        end
        """;

    readonly TimeSpan _expiry = TimeSpan.FromSeconds(opts.ExpirySec);
    readonly TimeSpan _retryPeriod = TimeSpan.FromSeconds(opts.RetryPeriodSec);

    static void EnsureExpiry(TimeSpan expiry)
    {
        if (expiry < TimeSpan.FromMilliseconds(1))
            throw new InvalidOperationException("Redlock expiry must be at least 1 millisecond.");
    }

    static void EnsureServers(IRedisServer[] servers)
    {
        if (servers.Any(s => s.ServerType == ServerType.Cluster))
        {
            throw new InvalidOperationException(
                "Redlock requires independent Redis masters");
        }
    }

    static int Quorum(int serverCount) => serverCount / 2 + 1;

    Task<bool> TryAcquireOnServerAsync(IRedisServer server, RedisKey key, string token, TimeSpan expiry)
        => ExecuteServerAsync(() =>
            ServerKeysTool.StringSetAsync(server, key, token, expiry, When.NotExists, opts.DbIndex));

    Task ReleaseOnServerAsync(IRedisServer server, RedisKey key, string token)
        => ExecuteServerAsync(async () =>
        {
            await ServerKeysTool.ScriptEvaluateAsync(server, UnlockScript, [key], [token], opts.DbIndex);
            return true;
        });

    static async Task<bool> ExecuteServerAsync(Func<Task<bool>> action)
    {
        try
        {
            return await action();
        }
        catch (RedisException)
        {
            return false;
        }
    }

    static TimeSpan RemainingValidity(TimeSpan expiry, TimeSpan elapsed)
    {
        var drift = TimeSpan.FromMilliseconds(expiry.TotalMilliseconds * 0.01 + 2);
        return expiry - elapsed - drift;
    }

    static bool IsOne(RedisResult result)
        => !result.IsNull && result.Resp2Type == ResultType.Integer && (long)result == 1;
}