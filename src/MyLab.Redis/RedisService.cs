using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MyLab.Redis
{
    class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _connection;

        public RedisService(IOptions<RedisOptions> options)
            : this(options.Value)
        {

        }

        public RedisService(RedisOptions options)
        {
            var cs = new RedisConfigurationOptionsBuilder(options).Build();

            _connection = ConnectionMultiplexer.Connect(cs);
        }

        public RedisDbKeysProvider Keys()
        {
            return new RedisDbKeysProvider(_connection.GetDatabase());
        }

        public RedisServerToolsProvider Server()
        {
            var defaultEndpoint = _connection.GetEndPoints().First();
            return Server(defaultEndpoint);
        }

        public RedisServerToolsProvider Server(EndPoint endPoint)
        {
            var defaultServer = _connection.GetServer(endPoint);
            return new RedisServerToolsProvider(defaultServer);
        }
    }
}
