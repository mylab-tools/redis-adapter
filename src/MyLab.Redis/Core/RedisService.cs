using MyLab.Redis.Abstractions;
using StackExchange.Redis;
using System.Net;
using Microsoft.Extensions.Options;
using MyLab.Redis.Locking;
using MyLab.Redis.Options;

namespace MyLab.Redis.Core;

class RedisService(IOptions<RedisOptions> opts) : IRedisConnectionProvider, IRedisConnectionStorageCore, IRedisService
{
    private IConnectionMultiplexer? _connectionMultiplexer;

    public bool Connected => _connectionMultiplexer?.IsConnected ?? false;

    public IRedisServer Server()
    {
        CheckConnection();

        var innerServer = _connectionMultiplexer!.GetServer(
            _connectionMultiplexer.GetEndPoints().First());
        return new RedisServer(innerServer);
    }

    public IRedisServer Server(EndPoint endpoint)
    {
        CheckConnection();

        var innerServer = _connectionMultiplexer!.GetServer(endpoint);
        return new RedisServer(innerServer);
    }

    public IRedisServer Server(string hostAndPort)
    {
        CheckConnection();

        var innerServer = _connectionMultiplexer!.GetServer(hostAndPort);
        return new RedisServer(innerServer);
    }

    public IRedisServer Server(string host, int port)
    {
        CheckConnection();

        var innerServer = _connectionMultiplexer!.GetServer(host, port);
        return new RedisServer(innerServer);
    }

    public IRedisServer[] GetAllServers()
    {
        CheckConnection();

        return _connectionMultiplexer!.GetServers()
            .Select(s => new RedisServer(s))
            .Cast<IRedisServer>()
            .ToArray();
    }

    public IRedisDb Db(int dbIndex0 = -1)
    {
        CheckConnection();

        return new RedisDb(_connectionMultiplexer!.GetDatabase(dbIndex0));
    }

    public IRedisLocker Lock(string lockId)
    {
        return new RedlockLocker(lockId, Masters(), opts.Value.Redlock ?? new RedlockOptions());
    }

    public IConnectionMultiplexer? Provide()
    {
        return _connectionMultiplexer;
    }

    public void SaveConnection(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    void CheckConnection()
    {
        if (_connectionMultiplexer == null)
        {
            throw new InvalidOperationException("Redis is not connected");
        }
    }

    IRedisServer[] Masters()
    {
        var servers = GetAllServers().Where(server => !server.IsReplica).ToArray();
        if (servers.Length == 0)
            throw new InvalidOperationException("Redis has no master server to lock.");

        return servers;
    }
}