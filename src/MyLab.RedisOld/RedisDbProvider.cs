using System.Threading.Tasks;
using MyLab.Redis.Connection;
using StackExchange.Redis;

namespace MyLab.Redis
{
    /// <summary>
    /// Provides Redis database object
    /// </summary>
    public class RedisDbProvider
    {
        private readonly IRedisConnectionProvider _connectionProvider;
        private readonly int _dbIndex;

        /// <summary>
        /// Initializes a new instance of <see cref="RedisDbProvider"/>
        /// </summary>
        public RedisDbProvider(IRedisConnectionProvider connectionProvider, int dbIndex = -1)
        {
            _connectionProvider = connectionProvider;
            _dbIndex = dbIndex;
        }

        /// <summary>
        /// Provides Redis database with index from ctor
        /// </summary>
        public IDatabase Provide()
        {
            var connection = _connectionProvider.Provide();
            return connection.GetDatabase(_dbIndex);
        }
    }
}
