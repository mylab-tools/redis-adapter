using StackExchange.Redis;

namespace MyLab.Redis.Abstractions;

/// <summary>
/// Commands addressed to one Redis server.
/// </summary>
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

    /// <summary>
    /// Round-trip time to the server.
    /// </summary>
    Task<TimeSpan> PingAsync();

    /// <summary>
    /// Echoes <paramref name="value"/> back from the server.
    /// </summary>
    /// <param name="value">Value to send.</param>
    Task<RedisValue> EchoAsync(RedisValue value);

    /// <summary>
    /// Iterates keys of the server database that match <paramref name="pattern"/>.
    /// </summary>
    /// <param name="pattern">Glob pattern. <see langword="null"/> matches every key.</param>
    /// <param name="dbIndex">Database index. A negative value selects the configured default database.</param>
    /// <param name="batchSize">Hint for how many keys to request per <c>SCAN</c> call.</param>
    IAsyncEnumerable<RedisKey> GetKeys
    (
        string? pattern = null,
        int dbIndex = -1,
        int batchSize = 250
    );

    /// <summary>
    /// Deletes every key in every database of this server.
    /// </summary>
    Task FlushAllAsync();

    /// <summary>
    /// Underlying StackExchange.Redis server.
    /// </summary>
    IServer GetUnderlineServer();
}