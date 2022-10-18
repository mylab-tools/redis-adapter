using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.ObjectModel
{
    /// <summary>
    /// Represent Redis STRING key
    /// </summary>
    public class StringRedisKey : RedisKeyBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="StringRedisKey"/>
        /// </summary>
        public StringRedisKey(RedisDbProvider dbProvider, string keyName) 
            : base(dbProvider, keyName)
        {
        }

        /// <summary>
        /// Decrements the number stored at key by decrement. If the key does not exist, it is set to 0 before performing the operation.
        /// An error is returned if the key contains a value of the wrong type or contains a string that is not representable as integer. This operation is limited to 64 bit signed integers.
        /// </summary>
        /// <param name="value">The amount to decrement by (defaults to 1).</param>
        /// <returns>The value of key after the decrement.</returns>
        /// <remarks>https://redis.io/commands/decrby</remarks>
        /// <remarks>https://redis.io/commands/decr</remarks>
        public Task<long> DecrementAsync(long value = 1)
        {
            return RedisDb.StringDecrementAsync(KeyName, value);
        }

        /// <summary>
        /// Decrements the string representing a floating point number stored at key by the specified decrement. If the key does not exist, it is set to 0 before performing the operation. The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.
        /// </summary>
        /// <param name="value">The amount to decrement by (defaults to 1).</param>
        /// <returns>The value of key after the decrement.</returns>
        /// <remarks>https://redis.io/commands/incrbyfloat</remarks>
        public Task<double> DecrementAsync(double value)
        {
            return RedisDb.StringDecrementAsync(KeyName, value);
        }

        /// <summary>
        /// Get the value of key. If the key does not exist the special value nil is returned. An error is returned if the value stored at key is not a string, because GET only handles string values.
        /// </summary>
        /// <returns>The value of key, or nil when key does not exist.</returns>
        /// <remarks>https://redis.io/commands/get</remarks>
        public Task<RedisValue> GetAsync()
        {
            return RedisDb.StringGetAsync(KeyName);
        }

        /// <summary>
        /// Returns the substring of the string value stored at key, determined by the offsets start and end (both are inclusive). Negative offsets can be used in order to provide an offset starting from the end of the string. So -1 means the last character, -2 the penultimate and so forth.
        /// </summary>
        /// <param name="start">The start index of the substring to get.</param>
        /// <param name="end">The end index of the substring to get.</param>
        /// <returns>The substring of the string value stored at key.</returns>
        /// <remarks>https://redis.io/commands/getrange</remarks>
        public Task<RedisValue> GetRangeAsync(long start, long end)
        {
            return RedisDb.StringGetRangeAsync(KeyName, start, end);
        }

        /// <summary>
        /// Atomically sets key to value and returns the old value stored at key.
        /// </summary>
        /// <param name="value">The value to replace the existing value with.</param>
        /// <returns>The old value stored at key, or nil when key did not exist.</returns>
        /// <remarks>https://redis.io/commands/getset</remarks>
        public Task<RedisValue> GetSetAsync(RedisValue value)
        {
            return RedisDb.StringGetSetAsync(KeyName, value);
        }

        /// <summary>
        /// Increments the number stored at key by increment. If the key does not exist, it is set to 0 before performing the operation. An error is returned if the key contains a value of the wrong type or contains a string that is not representable as integer. This operation is limited to 64 bit signed integers.
        /// </summary>
        /// <param name="value">The amount to increment by (defaults to 1).</param>
        /// <returns>The value of key after the increment.</returns>
        /// <remarks>https://redis.io/commands/incrby</remarks>
        /// <remarks>https://redis.io/commands/incr</remarks>
        public Task<long> IncrementAsync(long value = 1)
        {
            return RedisDb.StringIncrementAsync(KeyName, value);
        }

        /// <summary>
        /// Increments the string representing a floating point number stored at key by the specified increment. If the key does not exist, it is set to 0 before performing the operation. The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.
        /// </summary>
        /// <param name="value">The amount to increment by (defaults to 1).</param>
        /// <returns>The value of key after the increment.</returns>
        /// <remarks>https://redis.io/commands/incrbyfloat</remarks>
        public Task<double> IncrementAsync(double value)
        {
            return RedisDb.StringIncrementAsync(KeyName, value);
        }

        /// <summary>
        /// Returns the length of the string value stored at key.
        /// </summary>
        /// <returns>the length of the string at key, or 0 when key does not exist.</returns>
        /// <remarks>https://redis.io/commands/strlen</remarks>
        public Task<long> LengthAsync()
        {
            return RedisDb.StringLengthAsync(KeyName);
        }

        /// <summary>
        /// Set key to hold the string value. If key already holds a value, it is overwritten, regardless of its type.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns>`true` - if the string was set, `false` - otherwise.</returns>
        /// <remarks>https://redis.io/commands/set</remarks>
        public Task<bool> SetAsync(RedisValue value)
        {
            return RedisDb.StringSetAsync(KeyName, value);
        }

        /// <summary>
        /// Set key to hold string value if key does not exist.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <param name="expiry">The key expiry.</param>
        /// <returns>`true` - if the string was set, `false` - otherwise.</returns>
        /// <remarks>https://redis.io/commands/setnx/</remarks>
        public Task<bool> SetIfNotExistsAsync(RedisValue value, TimeSpan expiry)
        {
            return RedisDb.StringSetAsync(KeyName, value, expiry, When.NotExists);
        }

        /// <summary>
        /// Overwrites part of the string stored at key, starting at the specified offset, for the entire length of value. If the offset is larger than the current length of the string at key, the string is padded with zero-bytes to make offset fit. Non-existing keys are considered as empty strings, so this command will make sure it holds a string large enough to be able to set value at offset.
        /// </summary>
        /// <param name="offset">The offset in the string to overwrite.</param>
        /// <param name="value">The value to overwrite with.</param>
        /// <returns>The length of the string after it was modified by the command.</returns>
        /// <remarks>https://redis.io/commands/setrange</remarks>
        public Task<RedisValue> SetRangeAsync(long offset, RedisValue value)
        {
            return RedisDb.StringSetRangeAsync(KeyName, offset, value);
        }
    }
}