using MyLab.Redis.Core;
using StackExchange.Redis;
using System.Net;

namespace MyLab.Redis.Abstractions;

public interface IRedisService
{
    bool Connected { get; }

    IRedisServer GetServer();

    IRedisServer GetServer(EndPoint endpoint);

    IRedisServer GetServer(string hostAndPort);

    IRedisServer GetServer(string host, int port);

    IRedisServer[] GetServers();
}

public interface IRedisServer
{
    Task<TimeSpan> PingAsync();

    Task<RedisValue> EchoAsync(RedisValue value);

    RedisKeyEnumerable GetKeys
    (
        long cursor0,
        string? pattern = null,
        int count = 250,
        int offset = 0
    );

    Task FlushAllAsync();
}