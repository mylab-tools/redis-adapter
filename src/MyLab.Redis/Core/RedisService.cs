using MyLab.Redis.Abstractions;
using StackExchange.Redis;

namespace MyLab.Redis.Core
{
    class RedisService : IRedisConnectionProvider, IRedisConnectionStorageCore, IRedisService
    {
        private IConnectionMultiplexer? _connectionMultiplexer;

        public bool Connected => _connectionMultiplexer?.IsConnected ?? false;

        public IConnectionMultiplexer? Provide()
        {
            return _connectionMultiplexer;
        }

        public void SaveConnection(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
    }
}
