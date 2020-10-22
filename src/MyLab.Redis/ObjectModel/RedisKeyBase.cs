using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.ObjectModel
{
    /// <summary>
    /// The base class for Redis keys
    /// </summary>
    public class RedisKeyBase
    {
        protected readonly IDatabaseAsync RedisDb;
        protected readonly string KeyName;

        public RedisKeyBase(IDatabaseAsync redisDb, string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
                throw new ArgumentException("Value cannot be null or empty.", nameof(keyName));
            RedisDb = redisDb ?? throw new ArgumentNullException(nameof(redisDb));
            KeyName = keyName;
        }

        /// <summary>
        /// Removes the specified key. A key is ignored if it does not exist. If UNLINK is available (Redis 4.0+), it will be used.
        /// </summary>
        public async Task DeleteKeyAsync()
        {
            await RedisDb.KeyDeleteAsync(KeyName);
        }

        /// <summary>
        /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted. A key with an associated timeout is said to be volatile in Redis terminology.
        /// </summary>
        public async Task SetExpiryAsync(DateTime expiry)
        {
            await PerformAndCheckSuccessful(RedisDb.KeyExpireAsync(KeyName, expiry));
        }

        /// <summary>
        /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted. A key with an associated timeout is said to be volatile in Redis terminology.
        /// </summary>
        public async Task SetExpiryAsync(TimeSpan expiry)
        {
            await PerformAndCheckSuccessful(RedisDb.KeyExpireAsync(KeyName, expiry));
        }

        /// <summary>
        /// Returns true if key exists.
        /// </summary>
        public async Task<bool> KeyExistsAsync()
        {
            return await RedisDb.KeyExistsAsync(KeyName);
        }

        protected async Task<T> PerformAndCheckSuccessful<T>(Task<T> performTask)
        {
            var succ = await performTask;
            if (succ.Equals(default(T)))
                throw new RedisOperationException();

            return succ;
        }

        protected async Task<RedisValue> PerformAndCheckSuccessful(Task<RedisValue> performTask)
        {
            var res = await performTask;
            if (res.IsNull)
                throw new RedisOperationException();

            return res;
        }
    }
}
