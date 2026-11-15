using MyLab.Redis.Abstractions;
using StackExchange.Redis;
using System.Net;
using MyLab.Redis.Keys;

namespace MyLab.Redis.Core
{
    class RedisService : IRedisConnectionProvider, IRedisConnectionStorageCore, IRedisService
    {
        private IConnectionMultiplexer? _connectionMultiplexer;

        public bool Connected => _connectionMultiplexer?.IsConnected ?? false;

        public IRedisServer GetServer()
        {
            CheckConnection();

            var innerServer = _connectionMultiplexer!.GetServer(
                _connectionMultiplexer.GetEndPoints().First());
            return new RedisServer(innerServer);
        }

        public IRedisServer GetServer(EndPoint endpoint)
        {
            CheckConnection();

            var innerServer = _connectionMultiplexer!.GetServer(endpoint);
            return new RedisServer(innerServer);
        }

        public IRedisServer GetServer(string hostAndPort)
        {
            CheckConnection();

            var innerServer = _connectionMultiplexer!.GetServer(hostAndPort);
            return new RedisServer(innerServer);
        }

        public IRedisServer GetServer(string host, int port)
        {
            CheckConnection();

            var innerServer = _connectionMultiplexer!.GetServer(host, port);
            return new RedisServer(innerServer);
        }

        public IRedisServer[] GetServers()
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
    }
}
