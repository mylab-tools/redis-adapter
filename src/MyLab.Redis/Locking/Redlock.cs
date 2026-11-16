using MyLab.Redis.Abstractions;
using MyLab.Redis.Options;
using System.Diagnostics;
using StackExchange.Redis;

namespace MyLab.Redis.Locking;

partial class Redlock(IRedisServer[] servers, RedlockOptions opts)
{
    public async Task<AcquireResult?> TryAcquireAsync(RedisKey key, CancellationToken cancellationToken)
    {
        EnsureExpiry(_expiry);

        if (_retryPeriod < TimeSpan.Zero)
            throw new InvalidOperationException("Redlock retry period cannot be negative.");

        cancellationToken.ThrowIfCancellationRequested();

        var quorum = Quorum(servers.Length);
        var token = Guid.NewGuid().ToString("N");
        var started = Stopwatch.GetTimestamp();
        var acquired = 0;

        try
        {
            foreach (var server in servers)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (await TryAcquireOnServerAsync(server, key, token, _expiry))
                    acquired++;
            }
        }
        catch
        {
            await ReleaseAsync(key, token);
            throw;
        }

        var validity = RemainingValidity(_expiry, Stopwatch.GetElapsedTime(started));
        if (acquired >= quorum && validity > TimeSpan.Zero)
            return new AcquireResult(key, token, validity);

        await ReleaseAsync(key, token);
        return null;
    }

    public Task ReleaseAsync(RedisKey key, string token)
        => Task.WhenAll(servers.Select(server => ReleaseOnServerAsync(server, key, token)));

    public async Task<bool> IsHeldAsync(RedisKey key, string token)
    {
        var quorum = Quorum(servers.Length);
        var held = 0;

        foreach (var server in servers)
        {
            var owns = await ExecuteServerAsync(async () =>
            {
                var value = await ServerKeysTool.StringGetAsync(server, key, opts.DbIndex);
                return value == token;
            });

            if (owns)
                held++;
        }

        return held >= quorum;
    }

    public async Task<TimeSpan?> ProlongAsync(RedisKey key, string token)
    {
        EnsureExpiry(_expiry);

        var quorum = Quorum(servers.Length);
        var started = Stopwatch.GetTimestamp();
        var prolonged = 0;

        foreach (var server in servers)
        {
            var extended = await ExecuteServerAsync(async () =>
            {
                var result = await ServerKeysTool.ScriptEvaluateAsync(
                    server,
                    ProlongScript,
                    [key],
                    [token, (long)_expiry.TotalMilliseconds],
                    opts.DbIndex);
                return IsOne(result);
            });

            if (extended)
                prolonged++;
        }

        var validity = RemainingValidity(_expiry, Stopwatch.GetElapsedTime(started));
        return prolonged >= quorum && validity > TimeSpan.Zero ? validity : null;
    }
}