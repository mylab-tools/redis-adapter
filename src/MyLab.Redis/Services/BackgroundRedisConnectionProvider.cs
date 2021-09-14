using StackExchange.Redis;

namespace MyLab.Redis.Services
{
    class BackgroundRedisConnectionProvider : IRedisConnectionProvider
    {
        private readonly IBackgroundRedisConnectionManager _connectionManager;

        public BackgroundRedisConnectionProvider(IBackgroundRedisConnectionManager connectionManager)
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