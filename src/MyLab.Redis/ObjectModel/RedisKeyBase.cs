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
        private readonly RedisDbProvider _dbProvider;

        /// <summary>
        /// The key name
        /// </summary>
        public string KeyName { get; }

        /// <summary>
        /// Redis DB reference
        /// </summary>
        protected IDatabase RedisDb => _dbProvider.Provide();

        /// <summary>
        /// Initializes a new instance of <see cref="RedisKeyBase"/>
        /// </summary>
        protected RedisKeyBase(RedisDbProvider dbProvider, string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
                throw new ArgumentException("Value cannot be null or empty.", nameof(keyName));
            _dbProvider = dbProvider ?? throw new ArgumentNullException(nameof(dbProvider));
            KeyName = keyName;
        }

        /// <summary>
        /// Removes the specified key. A key is ignored if it does not exist.
        /// If UNLINK is available (Redis 4.0+), it will be used.
        /// </summary>
        /// <remarks>https://redis.io/commands/del</remarks>
        /// <remarks>https://redis.io/commands/unlink</remarks>
        public Task DeleteAsync()
        {
            return RedisDb.KeyDeleteAsync(KeyName);
        }

        /// <summary>
        /// Serialize the value stored at key in a Redis-specific format and return it to the user. The returned value can be synthesized back into a Redis key using the RESTORE command.
        /// </summary>
        /// <param name="key">The key to dump.</param>
        /// <returns>the serialized value.</returns>
        /// <remarks>https://redis.io/commands/dump</remarks>
        public Task<byte[]> DumpAsync(StackExchange.Redis.RedisKey key)
        {
            return RedisDb.KeyDumpAsync(KeyName);
        }

        /// <summary>
        /// Returns if key exists.
        /// </summary>
        /// <returns>1 if the key exists. 0 if the key does not exist.</returns>
        /// <remarks>https://redis.io/commands/exists</remarks>
        public Task<bool> ExistsAsync()
        {
            return RedisDb.KeyExistsAsync(KeyName);
        }

        /// <summary>
        /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted. A key with an associated timeout is said to be volatile in Redis terminology.
        /// </summary>
        /// <param name="expiry">The timeout to set.</param>
        /// <returns>`true` - if the timeout was set. `false` - if key does not exist or the timeout could not be set.</returns>
        /// <remarks>If key is updated before the timeout has expired, then the timeout is removed as if the PERSIST command was invoked on key.
        /// For Redis versions &lt; 2.1.3, existing timeouts cannot be overwritten. So, if key already has an associated timeout, it will do nothing and return 0. Since Redis 2.1.3, you can update the timeout of a key. It is also possible to remove the timeout using the PERSIST command. See the page on key expiry for more information.</remarks>
        /// <remarks>https://redis.io/commands/expire</remarks>
        /// <remarks>https://redis.io/commands/pexpire</remarks>
        /// <remarks>https://redis.io/commands/persist</remarks>
        public Task<bool> ExpireAsync(TimeSpan? expiry)
        {
            return RedisDb.KeyExpireAsync(KeyName, expiry);
        }

        /// <summary>
        /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted. A key with an associated timeout is said to be volatile in Redis terminology.
        /// </summary>
        /// <param name="expiry">The exact date to expiry to set.</param>
        /// <returns>`true` - if the timeout was set. `false` - if key does not exist or the timeout could not be set.</returns>
        /// <remarks>If key is updated before the timeout has expired, then the timeout is removed as if the PERSIST command was invoked on key.
        /// For Redis versions &lt; 2.1.3, existing timeouts cannot be overwritten. So, if key already has an associated timeout, it will do nothing and return 0. Since Redis 2.1.3, you can update the timeout of a key. It is also possible to remove the timeout using the PERSIST command. See the page on key expiry for more information.</remarks>
        /// <remarks>https://redis.io/commands/expireat</remarks>
        /// <remarks>https://redis.io/commands/pexpireat</remarks>
        /// <remarks>https://redis.io/commands/persist</remarks>
        public Task<bool> ExpireAsync(DateTime? expiry)
        {
            return RedisDb.KeyExpireAsync(KeyName, expiry);
        }

        /// <summary>
        /// Returns the absolute time at which the given key will expire, if it exists and has an expiration.
        /// </summary>
        /// <returns>The time at which the given key will expire, or <see langword="null"/> if the key does not exist or has no associated expiration time.</returns>
        /// <remarks>
        /// <seealso href="https://redis.io/commands/expiretime"/>,
        /// <seealso href="https://redis.io/commands/pexpiretime"/>
        /// </remarks>
        public async Task<DateTime?> GetExpirationAsync()
        {
            var universalDt = await RedisDb.KeyExpireTimeAsync(KeyName);

            return universalDt?.ToLocalTime();
        }

        /// <summary>
        /// Returns the time since the object stored at the specified key is idle (not requested by read or write operations)
        /// </summary>
        /// <returns>The time since the object stored at the specified key is idle</returns>
        /// <remarks>https://redis.io/commands/object</remarks>
        public Task<TimeSpan?> IdleTimeAsync()
        {
            return RedisDb.KeyIdleTimeAsync(KeyName);
        }

        /// <summary>
        /// Remove the existing timeout on key, turning the key from volatile (a key with an expire set) to persistent (a key that will never expire as no timeout is associated).
        /// </summary>
        /// <returns>`true` - if the timeout was removed. `false` - if key does not exist or does not have an associated timeout.</returns>
        /// <remarks>https://redis.io/commands/persist</remarks>
        public Task<bool> PersistAsync()
        {
            return RedisDb.KeyPersistAsync(KeyName);
        }


        /// <summary>
        /// Create a key associated with a value that is obtained by deserializing the provided serialized value (obtained via DUMP).
        /// If ttl is 0 the key is created without any expire, otherwise the specified expire time(in milliseconds) is set.
        /// </summary>
        /// <param name="key">The key to restore.</param>
        /// <param name="value">The value of the key.</param>
        /// <remarks>https://redis.io/commands/restore</remarks>
        public Task RestoreAsync(StackExchange.Redis.RedisKey key, byte[] value)
        {
            return RedisDb.KeyRestoreAsync(KeyName, value);
        }

        /// <summary>
        /// Returns the remaining time to live of a key that has a timeout.  This introspection capability allows a Redis client to check how many seconds a given key will continue to be part of the dataset.
        /// </summary>
        /// <returns>TTL, or nil when key does not exist or does not have a timeout.</returns>
        /// <remarks>https://redis.io/commands/ttl</remarks>
        public Task<TimeSpan?> TimeToLiveAsync()
        {
            return RedisDb.KeyTimeToLiveAsync(KeyName);
        }

        /// <summary>
        /// Returns the string representation of the type of the value stored at key. The different types that can be returned are: string, list, set, zset and hash.
        /// </summary>
        /// <returns>Type of key, or none when key does not exist.</returns>
        /// <remarks>https://redis.io/commands/type</remarks>
        public Task<RedisType> TypeAsync()
        {
            return RedisDb.KeyTypeAsync(KeyName);
        }

        /// <summary>
        /// Touch the specified key. 
        /// </summary>
        /// <returns>True if the key was touched.</returns>
        /// <remarks>https://redis.io/commands/touch</remarks>
        public Task<bool> TouchAsync()
        {
            return RedisDb.KeyTouchAsync(KeyName);
        }
    }
}
