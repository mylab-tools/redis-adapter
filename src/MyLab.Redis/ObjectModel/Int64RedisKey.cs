using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.ObjectModel
{
    /// <summary>
    /// String key with numeric long value
    /// </summary>
    public class Int64RedisKey : RedisKeyBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Int64RedisKey"/>
        /// </summary>
        public Int64RedisKey(IDatabaseAsync redisDb, string keyName)
            : base(redisDb, keyName)
        {
        }

        /// <summary>
        /// Set key to hold the string value of long
        /// </summary>
        public async Task SetValueAsync(long value)
        {
            await PerformAndCheckSuccessful(RedisDb.StringSetAsync(KeyName, value, null, When.Always));
        }

        /// <summary>
        /// Get the value of key
        /// </summary>
        public async Task<long> GetValueAsync()
        {
            var res = await PerformAndCheckSuccessful(RedisDb.StringGetAsync(KeyName));
            if (!res.TryParse(out long lVal))
                throw new RedisOperationException($"Can't parse Int64 key value '{res}'");

            return lVal;
        }

        /// <summary>
        /// Increments the number stored at key by increment. If the key does not exist, it is set to 0 before performing the operation. An error is returned if the key contains a value of the wrong type or contains a string that is not representable as integer. This operation is limited to 64 bit signed integers.
        /// </summary>
        /// <returns>The value of key after the increment.</returns>
        public async Task<long> IncrementAsync(long delta = 1)
        {
            return await RedisDb.StringIncrementAsync(KeyName, delta);
        }

        /// <summary>
        /// Decrements the number stored at key by decrement. If the key does not exist, it is set to 0 before performing the operation. An error is returned if the key contains a value of the wrong type or contains a string that is not representable as integer. This operation is limited to 64 bit signed integers.
        /// </summary>
        /// <returns>The value of key after the increment.</returns>
        public async Task<long> DecrementAsync(long delta = 1)
        {
            return await RedisDb.StringDecrementAsync(KeyName, delta);
        }
    }
}