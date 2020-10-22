using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.ObjectModel
{
    /// <summary>
    /// String key with double value
    /// </summary>
    public class DoubleRedisKey : RedisKeyBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DoubleRedisKey"/>
        /// </summary>
        public DoubleRedisKey(IDatabaseAsync redisDb, string keyName)
            : base(redisDb, keyName)
        {
        }

        /// <summary>
        /// Set key to hold the string value of long
        /// </summary>
        public async Task SetValueAsync(double value)
        {
            await PerformAndCheckSuccessful(RedisDb.StringSetAsync(KeyName, value, null, When.Always));
        }

        /// <summary>
        /// Get the value of key
        /// </summary>
        public async Task<double> GetValueAsync()
        {
            var res = await PerformAndCheckSuccessful(RedisDb.StringGetAsync(KeyName));
            if (!res.TryParse(out double dVal))
                throw new RedisOperationException($"Can't parse Double key value '{res}'");

            return dVal;
        }

        /// <summary>
        /// Increments the string representing a floating point number stored at key by the specified increment. If the key does not exist, it is set to 0 before performing the operation. The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.
        /// </summary>
        /// <returns>The value of key after the increment.</returns>
        public async Task<double> IncrementAsync(double delta = 1)
        {
            return await RedisDb.StringIncrementAsync(KeyName, delta);
        }

        /// <summary>
        /// Decrements the string representing a floating point number stored at key by the specified decrement. If the key does not exist, it is set to 0 before performing the operation. The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.
        /// </summary>
        /// <returns>The value of key after the increment.</returns>
        public async Task<double> DecrementAsync(double delta = 1)
        {
            return await RedisDb.StringDecrementAsync(KeyName, delta);
        }
    }
}