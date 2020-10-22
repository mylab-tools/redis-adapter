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
        /// Gets string key
        /// </summary>
        public StringRedisKey String(string key)
        {
            return new StringRedisKey(_redisDb, key);
        }

        /// <summary>
        /// Gets string key with numeric (int64) content
        /// </summary>
        public Int64RedisKey Int64(string key)
        {
            return new Int64RedisKey(_redisDb, key);
        }

        /// <summary>
        /// Gets string key with numeric (double) content
        /// </summary>
        public DoubleRedisKey Double(string key)
        {
            return new DoubleRedisKey(_redisDb, key);
        }
    }
}