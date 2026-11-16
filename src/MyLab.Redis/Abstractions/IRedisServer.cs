using StackExchange.Redis;

namespace MyLab.Redis.Abstractions;

public interface IRedisServer
{
    /// <summary>
    /// Gets a value indicating whether this server is connected.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Gets a value indicating whether this server is a read-only replica.
    /// </summary>
    bool IsReplica { get; }

    Task<TimeSpan> PingAsync();

    Task<RedisValue> EchoAsync(RedisValue value);

    IAsyncEnumerable<RedisKey> GetKeys
    (
        string? pattern = null,
        int dbIndex = -1,
        int batchSize = 250
    );

    Task FlushAllAsync();

    IServer GetUnderlineServer();
}