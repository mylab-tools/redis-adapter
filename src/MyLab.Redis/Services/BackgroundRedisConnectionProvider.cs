using StackExchange.Redis;

namespace MyLab.Redis.Services
{
    class BackgroundRedisConnectionProvider : IRedisConnectionProvider
    {
        private readonly IRedisConnectionManager _connectionManager;

        public BackgroundRedisConnectionProvider(IRedisConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public IConnectionMultiplexer Provide()
        {
            var resultConnection = _connectionManager.ProvideConnection();

            return resultConnection ?? throw new RedisNotConnectedException();
        }
    }
}