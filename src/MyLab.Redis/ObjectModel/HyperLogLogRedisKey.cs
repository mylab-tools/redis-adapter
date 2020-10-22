using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.ObjectModel
{
    /// <summary>
    /// Represent Redis HYPERLOGLOG key
    /// </summary>
    public class HyperLogLogRedisKey : RedisKeyBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HyperLogLogRedisKey"/>
        /// </summary>
        public HyperLogLogRedisKey(IDatabaseAsync redisDb, string keyName) 
            : base(redisDb, keyName)
        {
        }

        /// <summary>
        /// Adds the element to the HyperLogLog data structure stored at the variable name specified as first argument.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>True if at least 1 HyperLogLog internal register was altered, false otherwise.</returns>
        /// <remarks>https://redis.io/commands/pfadd</remarks>
        public Task<bool> AddAsync(RedisValue value)
        {
            return RedisDb.HyperLogLogAddAsync(KeyName, value);
        }

        /// <summary>
        /// Adds all the element arguments to the HyperLogLog data structure stored at the variable name specified as first argument.
        /// </summary>
        /// <param name="values">The values to add.</param>
        /// <returns>True if at least 1 HyperLogLog internal register was altered, false otherwise.</returns>
        /// <remarks>https://redis.io/commands/pfadd</remarks>
        public Task<bool> AddAsync(RedisValue[] values)
        {
            return RedisDb.HyperLogLogAddAsync(KeyName, values);
        }

        /// <summary>
        /// Returns the approximated cardinality computed by the HyperLogLog data structure stored at the specified variable, or 0 if the variable does not exist.
        /// </summary>
        /// <returns>The approximated number of unique elements observed via HyperLogLogAdd.</returns>
        /// <remarks>https://redis.io/commands/pfcount</remarks>
        public Task<long> LengthAsync()
        {
            return RedisDb.HyperLogLogLengthAsync(KeyName);
        }
    }
}