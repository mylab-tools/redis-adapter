using System;
using System.Threading.Tasks;
using MyLab.Redis.ObjectModel;
using MyLab.Redis.Scripting;
using StackExchange.Redis;

namespace MyLab.Redis
{
    /// <summary>
    /// Provides Redis tools for database
    /// </summary>
    public class RedisDbToolsProvider : RedisDbKeysProvider
    {
        private readonly RedisDbProvider _redisDbProvider;
        private readonly RedisCacheFactory _redisCacheFactory;

        public RedisDbToolsProvider(RedisDbProvider dbProvider, RedisCacheFactory redisCacheFactory)
            :base(dbProvider)
        {
            _redisDbProvider = dbProvider;
            _redisCacheFactory = redisCacheFactory;
        }

        /// <summary>
        /// Provides Redis base cache by name
        /// </summary>
        public RedisCache Cache(string name)
        {
            return _redisCacheFactory.Provide(name);
        }

        /// <summary>
        /// Creates script tools object
        /// </summary>
        /// <returns></returns>
        public RedisScriptTools Script()
        {
            return new RedisScriptTools(_redisDbProvider);
        }
    }
}