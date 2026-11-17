using MyLab.Redis.Abstractions;
using StackExchange.Redis;
using System.Net;

namespace MyLab.Redis.Core;

class RedisServer(IServer redisServer) : IRedisServer
{
    public EndPoint EndPoint => redisServer.EndPoint;
    public bool IsConnected => redisServer.IsConnected;
    public bool IsReplica => redisServer.IsReplica;

    public Task<TimeSpan> PingAsync() => redisServer.PingAsync();

    public Task<RedisValue> EchoAsync(RedisValue value) => redisServer.EchoAsync(value);
    public IAsyncEnumerable<RedisKey> GetKeysAsync
    (
        string? pattern = null,
        int dbIndex = -1,
        int batchSize = 250
    )
    {
        RedisValue redisPattern = pattern ?? default(RedisValue);
        return redisServer.KeysAsync
        (
            pattern: redisPattern,
            pageSize: batchSize,
            database: dbIndex
        );
    }

    public Task FlushAllAsync() => redisServer.FlushAllDatabasesAsync();

    public IServer GetUnderlineServer() => redisServer;

    static int? DatabaseOrDefault(int dbIndex) => dbIndex < 0 ? null : dbIndex;
}