using MyLab.Redis.Core;
using StackExchange.Redis;

namespace MyLab.Redis.Abstractions;

public interface IRedisServer
{
    Task<TimeSpan> PingAsync();

    Task<RedisValue> EchoAsync(RedisValue value);

    ScanningEnumerable<RedisKey> GetKeys
    (
        long cursor0,
        string? pattern = null,
        int count = 250,
        int offset = 0,
        int dbIndex = 0
    );

    Task FlushAllAsync();
}