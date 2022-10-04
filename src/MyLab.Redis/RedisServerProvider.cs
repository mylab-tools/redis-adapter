using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MyLab.Redis.Connection;
using StackExchange.Redis;

namespace MyLab.Redis
{
    /// <summary>
    /// Provides Redis server object
    /// </summary>
    public class RedisServerProvider
    {
        private readonly IRedisConnectionProvider _connectionProvider;
        private readonly EndPoint _endpoint;

        /// <summary>
        /// Initializes a new instance of <see cref="RedisServerProvider"/>
        /// </summary>
        public RedisServerProvider(IRedisConnectionProvider connectionProvider, EndPoint endpoint = null)
        {
            _connectionProvider = connectionProvider;
            _endpoint = endpoint;
        }

        /// <summary>
        /// Provides Redis server object
        /// </summary>
        public IServer Provide()
        {
            var connection = _connectionProvider.Provide();

            return connection.GetServer(
                _endpoint ??
                connection.GetEndPoints().First()
            );
        }
    }
}