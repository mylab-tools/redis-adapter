using System;
using MyLab.Redis.ObjectModel;
using StackExchange.Redis;

namespace MyLab.Redis
{
    public class RedisDbKeysProvider
    {
        private readonly RedisDbProvider _dbProvider;

        public RedisDbKeysProvider(RedisDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        /// <summary>
        /// Gets STRING key
        /// </summary>
        public StringRedisKey String(string key)
        {
            return new StringRedisKey(_dbProvider, key);
        }

        /// <summary>
        /// Gets HASH key
        /// </summary>
        public HashRedisKey Hash(string key)
        {
            return new HashRedisKey(_dbProvider, key);
        }

        /// <summary>
        /// Gets SET key
        /// </summary>
        public SetRedisKey Set(string key)
        {
            return new SetRedisKey(_dbProvider, key);
        }

        /// <summary>
        /// Gets SORTED SET key
        /// </summary>
        public SortedSetRedisKey SortedSet(string key)
        {
            return new SortedSetRedisKey(_dbProvider, key);
        }

        /// <summary>
        /// Gets LIST key
        /// </summary>
        public ListRedisKey List(string key)
        {
            return new ListRedisKey(_dbProvider, key);
        }
    }
}