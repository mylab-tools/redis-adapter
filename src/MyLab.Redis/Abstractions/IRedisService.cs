using MyLab.Redis.Options;
using System.Net;
using MyLab.Redis.Caching;
using MyLab.Redis.Locking;

namespace MyLab.Redis.Abstractions;

/// <summary>
/// Entry point for the current Redis connection.
/// </summary>
public interface IRedisService
{
    /// <summary>
    /// <see langword="true"/> when the connection to Redis is established.
    /// </summary>
    bool Connected { get; }

    /// <summary>
    /// Server selected by the connection.
    /// </summary>
    IRedisServer Server();

    /// <summary>
    /// Server at <paramref name="endpoint"/>.
    /// </summary>
    /// <param name="endpoint">Server endpoint.</param>
    IRedisServer Server(EndPoint endpoint);

    /// <summary>
    /// Server at <paramref name="hostAndPort"/>.
    /// </summary>
    /// <param name="hostAndPort">Server address in <c>host:port</c> form.</param>
    IRedisServer Server(string hostAndPort);

    /// <summary>
    /// Server at <paramref name="host"/> and <paramref name="port"/>.
    /// </summary>
    /// <param name="host">Server host.</param>
    /// <param name="port">Server port.</param>
    IRedisServer Server(string host, int port);

    /// <summary>
    /// All servers known to the connection.
    /// </summary>
    IRedisServer[] GetAllServers();

    /// <summary>
    /// Database <paramref name="dbIndex0"/>.
    /// </summary>
    /// <param name="dbIndex0">Database index. A negative value selects the configured default database.</param>
    IRedisDb Db(int dbIndex0 = -1);

    /// <summary>
    /// Distributed lock for <paramref name="lockId"/>.
    /// </summary>
    /// <param name="lockId">Lock identifier. The Redis key is <c>{BaseKey}:{lockId}</c>.</param>
    IRedisLocker Lock(string lockId);

    /// <summary>
    /// Cache configured by <paramref name="cacheId"/>.
    /// </summary>
    /// <param name="cacheId">Cache identifier matched against <see cref="ConfigCacheOptions.Overrides"/>.</param>
    IRedisCache Cache(string cacheId);
}