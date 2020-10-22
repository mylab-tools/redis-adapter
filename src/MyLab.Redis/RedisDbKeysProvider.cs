using MyLab.Redis.ObjectModel;
using StackExchange.Redis;

namespace MyLab.Redis
{
    /// <summary>
    /// Provides Redis key object model
    /// </summary>
    public class RedisDbKeysProvider
    {
        private readonly IDatabase _redisDb;

        public RedisDbKeysProvider(IDatabase redisDb)
        {
            _redisDb = redisDb;
        }

        /// <summary>
        /// Gets STRING key
        /// </summary>
        public StringRedisKey String(string key)
        {
            return new StringRedisKey(_redisDb, key);
        }

        /// <summary>
        /// Gets HASH key
        /// </summary>
        public HashRedisKey Hash(string key)
        {
            return new HashRedisKey(_redisDb, key);
        }

        /// <summary>
        /// Gets SET key
        /// </summary>
        public SetRedisKey Set(string key)
        {
            return new SetRedisKey(_redisDb, key);
        }

        /// <summary>
        /// Gets SORTED SET key
        /// </summary>
        public SortedSetRedisKey SortedSet(string key)
        {
            return new SortedSetRedisKey(_redisDb, key);
        }

        /// <summary>
        /// Gets LIST key
        /// </summary>
        public ListRedisKey List(string key)
        {
            return new ListRedisKey(_redisDb, key);
        }
    }
}