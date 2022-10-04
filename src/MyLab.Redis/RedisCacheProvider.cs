using System;
using System.Linq;
using MyLab.Redis.ObjectModel;

namespace MyLab.Redis
{
    /// <summary>
    /// Provides Redis cache
    /// </summary>
    public class RedisCacheProvider
    {
        private readonly RedisDbLink _database;
        private readonly RedisOptions _options;

        /// <summary>
        /// Initializes a new instance of <see cref="RedisCacheProvider"/>
        /// </summary>
        public RedisCacheProvider(RedisDbLink database, RedisOptions options)
        {
            _database = database;
            _options = options;
        }

        /// <summary>
        /// Provides <see cref="RedisCache"/> object
        /// </summary>
        public RedisCache Provide(string name)
        {
            var opt = _options.Cache?.FirstOrDefault(o => o.Name == name);
            if (opt == null)
                throw new InvalidOperationException($"RedisCache '{name}' not found");

            var defaultExpiry = OptionsExpiryParser.Parse(opt.DefaultExpiry);

            return new RedisCache(_database, opt.Key)
            {
                DefaultExpiry = defaultExpiry
            };
        }
    }
}