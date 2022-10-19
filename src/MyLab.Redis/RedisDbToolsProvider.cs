using MyLab.Redis.ObjectModel;
using MyLab.Redis.Scripting;

namespace MyLab.Redis
{
    /// <summary>
    /// Provides Redis tools for database
    /// </summary>
    public class RedisDbToolsProvider : RedisDbKeysProvider
    {
        private readonly RedisDbProvider _redisDbProvider;
        private readonly RedisCacheFactory _redisCacheFactory;
        private readonly RedlockerFactory _redlockerFactory;

        /// <summary>
        /// Create new instance of <see cref="RedisDbToolsProvider"/>
        /// </summary>
        public RedisDbToolsProvider(
            RedisDbProvider dbProvider, 
            RedisCacheFactory redisCacheFactory,
            RedlockerFactory redlockerFactory)
            : base(dbProvider)
        {
            _redisDbProvider = dbProvider;
            _redisCacheFactory = redisCacheFactory;
            _redlockerFactory = redlockerFactory;
        }

        /// <summary>
        /// Provides Redis-based cache by name
        /// </summary>
        public RedisCache Cache(string name)
        {
            return _redisCacheFactory.Create(name);
        }

        /// <summary>
        /// Provides Redis-based locker by name
        /// </summary>
        public Redlocker CreateLocker(string lockName)
        {
            return _redlockerFactory.Create(lockName);
        }

        /// <summary>
        /// Provides Redis-based locker by name
        /// </summary>
        public Redlocker CreateLocker(string lockName, string childId)
        {
            return _redlockerFactory.Create(lockName, childId);
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