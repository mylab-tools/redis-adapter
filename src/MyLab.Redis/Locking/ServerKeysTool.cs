using MyLab.Redis.Abstractions;
using StackExchange.Redis;

namespace MyLab.Redis.Locking;

internal static class ServerKeysTool
{
    public static Task<bool> StringSetAsync(
        IRedisServer redisServer,
        RedisKey key,
        RedisValue value,
        TimeSpan? expiry = null,
        When when = When.Always,
        int dbIndex = -1)
    {
        List<object> args = [key, value];

        switch (when)
        {
            case When.Exists:
                args.Add("XX");
                break;
            case When.NotExists:
                args.Add("NX");
                break;
            case When.Always:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(when), when, null);
        }

        if (expiry != null)
        {
            args.Add("PX");
            args.Add((long)expiry.Value.TotalMilliseconds);
        }

        return SetCoreAsync();

        async Task<bool> SetCoreAsync()
        {
            var result = await redisServer.GetUnderlineServer().ExecuteAsync(DatabaseOrDefault(dbIndex), "SET", args);
            return !result.IsNull && result.ToString() == "OK";
        }
    }

    public static async Task<RedisValue> StringGetAsync(IRedisServer redisServer, RedisKey key, int dbIndex = -1)
    {
        var result = await redisServer.GetUnderlineServer().ExecuteAsync(
            DatabaseOrDefault(dbIndex),
            "GET",
            [key]);

        if (result.IsNull)
            return RedisValue.Null;

        var bytes = (byte[]?)result;
        return bytes ?? RedisValue.Null;
    }

    public static Task<RedisResult> ScriptEvaluateAsync(
        IRedisServer redisServer,
        string script,
        RedisKey[]? keys = null,
        RedisValue[]? values = null,
        int dbIndex = -1)
    {
        keys ??= [];
        values ??= [];

        var args = new object[2 + keys.Length + values.Length];
        args[0] = script;
        args[1] = keys.Length;

        for (var i = 0; i < keys.Length; i++)
            args[2 + i] = keys[i];

        for (var i = 0; i < values.Length; i++)
            args[2 + keys.Length + i] = values[i];

        return redisServer.GetUnderlineServer().ExecuteAsync(DatabaseOrDefault(dbIndex), "EVAL", args);
    }

    static int? DatabaseOrDefault(int dbIndex) => dbIndex < 0 ? null : dbIndex;
}