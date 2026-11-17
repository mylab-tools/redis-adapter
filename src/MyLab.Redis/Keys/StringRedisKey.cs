using StackExchange.Redis;

namespace MyLab.Redis.Keys;

/// <summary>
/// Represent Redis STRING key
/// </summary>
public class StringRedisKey : RedisKeyBase
{
    /// <summary>
    /// Initializes a new instance of <see cref="StringRedisKey"/>
    /// </summary>
    public StringRedisKey(RedisKey key, IDatabaseAsync redisDb)
        : base(key, redisDb)
    {
    }

    /// <summary>
    /// Decrements the number stored at key by decrement.
    /// If the key does not exist, it is set to 0 before performing the operation.
    /// An error is returned if the key contains a value of the wrong type or contains a string that is not representable as integer.
    /// This operation is limited to 64-bit signed integers.
    /// </summary>
    /// <param name="value">The amount to decrement by (defaults to 1).</param>
    /// <returns>The value of key after the decrement.</returns>
    public Task<long> DecrementAsync(long value = 1)
        => RedisDb.StringDecrementAsync(Key, value);

    /// <summary>
    /// Decrements the string representing a floating point number stored at key by the specified decrement.
    /// If the key does not exist, it is set to 0 before performing the operation.
    /// The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.
    /// </summary>
    /// <param name="value">The amount to decrement by (defaults to 1).</param>
    /// <returns>The value of key after the decrement.</returns>
    public Task<double> DecrementAsync(double value)
        => RedisDb.StringDecrementAsync(Key, value);

    /// <summary>
    /// Get the value of key. If the key does not exist the special value nil is returned.
    /// An error is returned if the value stored at key is not a string, because GET only handles string values.
    /// </summary>
    /// <returns>The value of key, or nil when key does not exist.</returns>
    public Task<RedisValue> GetAsync()
        => RedisDb.StringGetAsync(Key);

    /// <summary>
    /// Returns the substring of the string value stored at key, determined by the offsets start and end (both are inclusive).
    /// Negative offsets can be used in order to provide an offset starting from the end of the string.
    /// So -1 means the last character, -2 the penultimate and so forth.
    /// </summary>
    /// <param name="start">The start index of the substring to get.</param>
    /// <param name="end">The end index of the substring to get.</param>
    /// <returns>The substring of the string value stored at key.</returns>
    public Task<RedisValue> GetRangeAsync(long start, long end)
        => RedisDb.StringGetRangeAsync(Key, start, end);

    /// <summary>
    /// Atomically sets key to value and returns the old value stored at key.
    /// </summary>
    /// <param name="value">The value to replace the existing value with.</param>
    /// <returns>The old value stored at key, or nil when key did not exist.</returns>
    public Task<RedisValue> GetSetAsync(RedisValue value)
        => RedisDb.StringGetSetAsync(Key, value);

    /// <summary>
    /// Increments the number stored at key by increment.
    /// If the key does not exist, it is set to 0 before performing the operation.
    /// An error is returned if the key contains a value of the wrong type or contains a string that is not representable as integer.
    /// This operation is limited to 64 bit signed integers.
    /// </summary>
    /// <param name="value">The amount to increment by (defaults to 1).</param>
    /// <returns>The value of key after the increment.</returns>
    public Task<long> IncrementAsync(long value = 1)
        => RedisDb.StringIncrementAsync(Key, value);

    /// <summary>
    /// Increments the string representing a floating point number stored at key by the specified increment.
    /// If the key does not exist, it is set to 0 before performing the operation.
    /// The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.
    /// </summary>
    /// <param name="value">The amount to increment by (defaults to 1).</param>
    /// <returns>The value of key after the increment.</returns>
    public Task<double> IncrementAsync(double value)
        => RedisDb.StringIncrementAsync(Key, value);

    /// <summary>
    /// Returns the length of the string value stored at key.
    /// </summary>
    /// <returns>the length of the string at key, or 0 when key does not exist.</returns>
    public Task<long> LengthAsync()
        => RedisDb.StringLengthAsync(Key);

    /// <summary>
    /// Set key to hold the string value. If key already holds a value, it is overwritten, regardless of its type.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <param name="expiry">The expiry to set.</param>
    /// <param name="when">Which condition to set the value under (defaults to always).</param>
    /// <returns><see langword="true"/> if the string was set, <see langword="false"/> otherwise.</returns>
    public Task<bool> SetAsync(RedisValue value, TimeSpan? expiry = null, When when = When.Always)
        => RedisDb.StringSetAsync(Key, value, expiry, when);

    /// <summary>
    /// Overwrites part of the string stored at key, starting at the specified offset, for the entire length of value.
    /// If the offset is larger than the current length of the string at key, the string is padded with zero-bytes to make offset fit.
    /// Non-existing keys are considered as empty strings, so this command will make sure it holds a string large enough to be able to set value at offset.
    /// </summary>
    /// <param name="offset">The offset in the string to overwrite.</param>
    /// <param name="value">The value to overwrite with.</param>
    /// <returns>The length of the string after it was modified by the command.</returns>
    public Task<RedisValue> SetRangeAsync(long offset, RedisValue value)
        => RedisDb.StringSetRangeAsync(Key, offset, value);
}
