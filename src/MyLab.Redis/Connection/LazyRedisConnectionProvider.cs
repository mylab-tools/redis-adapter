﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyLab.Log.Dsl;
using StackExchange.Redis;

namespace MyLab.Redis.Connection
{
    class LazyRedisConnectionProvider : IRedisConnectionProvider
    {
        private readonly RedisConnector _connector;
        private IConnectionMultiplexer _connection;
        private readonly object _connectionLock = new object();

        public LazyRedisConnectionProvider(IOptions<RedisOptions> options, ILogger<BackgroundRedisConnectionManager> logger = null)
            : this(options.Value, logger)
        {

        }

        public LazyRedisConnectionProvider(RedisOptions options, ILogger<BackgroundRedisConnectionManager> logger = null)
        {
            _connector = new RedisConnector(options)
            {
                Log = logger?.Dsl()
            };
        }

        public IConnectionMultiplexer Provide()
        {
            lock (_connectionLock)
            {
                if (_connection == null || !_connection.IsConnected)
                {
                    _connection?.Dispose();
                    _connection = _connector.Connect();
                }

                return _connection;
            }
        }
    }
}