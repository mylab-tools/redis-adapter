using System.Net;
using MyLab.Redis.Abstractions;
using StackExchange.Redis;

namespace MyLab.Redis.Core;

class RedisServer(IServer redisServer) : IRedisServer
{
    public EndPoint EndPoint => redisServer.EndPoint;
    public bool IsConnected => redisServer.IsConnected;
    public bool IsReplica => redisServer.IsReplica;

    public Task<TimeSpan> PingAsync() => redisServer.PingAsync();

    public Task<RedisValue> EchoAsync(RedisValue value) => redisServer.EchoAsync(value);

    public RedisKeyEnumerable GetKeys
    (
        long cursor0,
        string? pattern = null,
        int count = 250,
        int offset = 0
    )
    {
        RedisValue redisPattern = pattern ?? default(RedisValue);
        var keyEnumerable = redisServer.KeysAsync
        (
            pattern: redisPattern,
            pageSize: count,
            pageOffset: offset,
            cursor: cursor0
        );

        return new RedisKeyEnumerable(keyEnumerable);
    }

    public Task FlushAllAsync() => redisServer.FlushAllDatabasesAsync();

    public IServer GetUnderlineServer() => redisServer;
}