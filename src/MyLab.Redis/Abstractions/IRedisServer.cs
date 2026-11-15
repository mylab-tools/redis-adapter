using StackExchange.Redis;

namespace MyLab.Redis.Abstractions;

public interface IRedisServer
{
    Task<TimeSpan> PingAsync();

    Task<RedisValue> EchoAsync(RedisValue value);

    IAsyncEnumerable<RedisKey> GetKeys
    (
        string? pattern = null,
        int dbIndex = -1,
        int batchSize = 250
    );

    Task FlushAllAsync();
}