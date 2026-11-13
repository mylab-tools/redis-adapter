using MyLab.Redis.Abstractions;
using StackExchange.Redis;

namespace MyLab.Redis.Connection
{
    class RedisConnectionProvider : IRedisConnectionProvider, IRedisConnectionStorageCore
    {
        private IConnectionMultiplexer? _connectionMultiplexer;
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
