using System.Net;
using MyLab.Redis.Keys;

namespace MyLab.Redis.Abstractions;

public interface IRedisService
{
    bool Connected { get; }

    IRedisServer GetServer();

    IRedisServer GetServer(EndPoint endpoint);

    IRedisServer GetServer(string hostAndPort);

    IRedisServer GetServer(string host, int port);

    IRedisServer[] GetServers();

    IRedisDb Db(int dbIndex0 = -1);
}