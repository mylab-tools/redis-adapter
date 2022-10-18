using System;
using System.Linq;
using MyLab.Redis.ObjectModel;

namespace MyLab.Redis
{
    /// <summary>
    /// Creates Redis cache
    /// </summary>
    public class RedisCacheFactory
    {
        private readonly RedisDbLink _database;
        private readonly RedisOptions _options;

        /// <summary>
        /// Initializes a new instance of <see cref="RedisCacheFactory"/>
        /// </summary>
        public RedisCacheFactory(RedisDbLink database, RedisOptions options)
        {
            _database = database;
            _options = options;
        }

        /// <summary>
        /// Creates <see cref="RedisCache"/> object
        /// </summary>
        public RedisCache Create(string name)
        {
            var opt = _options.Caching?.Caches?.FirstOrDefault(o => o.Name == name);
            if (opt == null)
                throw new InvalidOperationException($"RedisCache '{name}' not found");

            var defaultExpiry = OptionsExpiryParser.Parse(opt.DefaultExpiry);

            var cacheKeyName = KeyNameTools.BuildName(_options.Caching.KeyPrefix, name);

            return new RedisCache(_database, cacheKeyName)
            {
                DefaultExpiry = defaultExpiry
            };
        }
    }
}