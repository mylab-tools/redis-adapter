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

    public ScanningEnumerable<RedisKey> GetKeys
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

        return new ScanningEnumerable<RedisKey>(keyEnumerable);
    }

    public Task FlushAllAsync() => redisServer.FlushAllDatabasesAsync();

    public IRedisDb Db(int dbIndex0 = -1) =>
        new RedisDb(redisServer.Multiplexer.GetDatabase(dbIndex0), redisServer);

    public IServer GetUnderlineServer() => redisServer;
}