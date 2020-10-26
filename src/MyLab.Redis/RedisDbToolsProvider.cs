using System;
using System.Threading.Tasks;
using MyLab.Redis.ObjectModel;
using StackExchange.Redis;

namespace MyLab.Redis
{
    /// <summary>
    /// Provides Redis tools for database
    /// </summary>
    public class RedisDbToolsProvider : RedisDbKeysProvider
    {
        private readonly IDatabase _redisDb;
        private readonly RedisCacheProvider _redisCacheProvider;

        public RedisDbToolsProvider(IDatabase redisDb, RedisCacheProvider redisCacheProvider)
            :base(redisDb)
        {
            _redisDb = redisDb;
            _redisCacheProvider = redisCacheProvider;
        }

        /// <summary>
        /// Provides Redis base cache by name
        /// </summary>
        public RedisCache Cache(string name)
        {
            return _redisCacheProvider.Provide(name);
        }

        /// <summary>
        /// Creates Redis transaction
        /// </summary>
        /// <returns></returns>
        public RedisTransaction BeginTransaction()
        {
            return new RedisTransaction(_redisDb.CreateTransaction());
        }

        /// <summary>
        /// Performs Redis transaction 
        /// </summary>
        /// <param name="act">transaction content</param>
        public Task PerformTransactionAsync(RedisTransactionAct act)
        {
            if (act == null) throw new ArgumentNullException(nameof(act));

            return InternalPerformTransactionAsync(act);
        }

        async Task InternalPerformTransactionAsync(RedisTransactionAct act)
        {
            await using var t = BeginTransaction();

            await act(t);
        }
    }

    /// <summary>
    /// Transaction action 
    /// </summary>
    public delegate Task RedisTransactionAct(RedisDbKeysProvider keys);
}