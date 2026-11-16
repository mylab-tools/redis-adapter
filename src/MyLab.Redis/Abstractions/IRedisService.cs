using System.Net;
using MyLab.Redis.Caching;
using MyLab.Redis.Locking;

namespace MyLab.Redis.Abstractions;

public interface IRedisService
{
    bool Connected { get; }

    IRedisServer Server();

    IRedisServer Server(EndPoint endpoint);

    IRedisServer Server(string hostAndPort);

    IRedisServer Server(string host, int port);

    IRedisServer[] GetAllServers();

    IRedisDb Db(int dbIndex0 = -1);

    IRedisLocker Lock(string lockId);

    IRedisCache Cache(string cacheId);
}