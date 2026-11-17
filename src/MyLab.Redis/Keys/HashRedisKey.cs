using MyLab.Redis.Core;
using StackExchange.Redis;

namespace MyLab.Redis.Keys;

/// <summary>
/// Represent Redis HASH
/// </summary>
public class HashRedisKey : RedisKeyBase
{
    /// <summary>
    /// Initializes a new instance of <see cref="HashRedisKey"/>
    /// </summary>
    public HashRedisKey(RedisKey key, IDatabaseAsync redisDb)
        : base(key, redisDb)
    {
    }

    /// <summary>
    /// Decrements the number stored at field in the hash stored at key by decrement.
    /// If key does not exist, a new key holding a hash is created.
    /// If field does not exist or holds a string that cannot be interpreted as integer, the value is set to 0 before the operation is performed.
    /// </summary>
    /// <param name="hashField">The field in the hash to decrement.</param>
    /// <param name="value">The amount to decrement by.</param>
    /// <returns>The value at field after the decrement operation.</returns>
    public Task<long> DecrementAsync(string hashField, long value = 1)
        => RedisDb.HashDecrementAsync(Key, hashField, value);

    /// <summary>
    /// Decrement the specified field of a hash stored at key, and representing a floating point number, by the specified decrement.
    /// If the field does not exist, it is set to 0 before performing the operation.
    /// The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.
    /// </summary>
    /// <param name="hashField">The field in the hash to decrement.</param>
    /// <param name="value">The amount to decrement by.</param>
    /// <returns>The value at field after the decrement operation.</returns>
    public Task<double> DecrementAsync(string hashField, double value)
        => RedisDb.HashDecrementAsync(Key, hashField, value);

    /// <summary>
    /// Removes the specified field from the hash stored at key.
    /// Non-existing fields are ignored. Non-existing keys are treated as empty hashes and this command returns 0.
    /// </summary>
    /// <param name="hashField">The field in the hash to delete.</param>
    /// <returns><see langword="true"/> if the field was removed.</returns>
    public Task<bool> DeleteFieldAsync(string hashField)
        => RedisDb.HashDeleteAsync(Key, hashField);

    /// <summary>
    /// Removes the specified fields from the hash stored at key.
    /// Non-existing fields are ignored. Non-existing keys are treated as empty hashes and this command returns 0.
    /// </summary>
    /// <param name="hashFields">The fields in the hash to delete.</param>
    /// <returns>The number of fields that were removed.</returns>
    public Task<long> DeleteFieldsAsync(string[] hashFields)
        => RedisDb.HashDeleteAsync(Key, hashFields.Select(field => (RedisValue)field).ToArray());

    /// <summary>
    /// Returns if field is an existing field in the hash stored at key.
    /// </summary>
    /// <param name="hashField">The field in the hash to check.</param>
    /// <returns>1 if the hash contains field. 0 if the hash does not contain field, or key does not exist.</returns>
    public Task<bool> FieldExistsAsync(string hashField)
        => RedisDb.HashExistsAsync(Key, hashField);

    /// <summary>
    /// Returns the value associated with field in the hash stored at key.
    /// </summary>
    /// <param name="hashField">The field in the hash to get.</param>
    /// <returns>The value associated with field, or nil when field is not present in the hash or key does not exist.</returns>
    public Task<RedisValue> GetAsync(string hashField)
        => RedisDb.HashGetAsync(Key, hashField);

    /// <summary>
    /// Returns the values associated with the specified fields in the hash stored at key.
    /// For every field that does not exist in the hash, a nil value is returned.
    /// Because non-existing keys are treated as empty hashes, running HMGET against a non-existing key will return a list of nil values.
    /// </summary>
    /// <param name="hashFields">The fields in the hash to get.</param>
    /// <returns>List of values associated with the given fields, in the same order as they are requested.</returns>
    public Task<RedisValue[]> GetAsync(string[] hashFields)
        => RedisDb.HashGetAsync(Key, hashFields.Select(field => (RedisValue)field).ToArray());

    /// <summary>
    /// Returns all fields and values of the hash stored at key.
    /// </summary>
    /// <returns>List of fields and their values stored in the hash, or an empty list when key does not exist.</returns>
    public Task<HashEntry[]> GetAllAsync()
        => RedisDb.HashGetAllAsync(Key);

    /// <summary>
    /// Increments the number stored at field in the hash stored at key by increment.
    /// If key does not exist, a new key holding a hash is created.
    /// If field does not exist or holds a string that cannot be interpreted as integer, the value is set to 0 before the operation is performed.
    /// The range of values supported by HINCRBY is limited to 64-bit signed integers.
    /// </summary>
    /// <param name="hashField">The field in the hash to increment.</param>
    /// <param name="value">The amount to increment by.</param>
    /// <returns>The value at field after the increment operation.</returns>
    public Task<long> IncrementAsync(string hashField, long value = 1)
        => RedisDb.HashIncrementAsync(Key, hashField, value);

    /// <summary>
    /// Increment the specified field of a hash stored at key, and representing a floating point number, by the specified increment.
    /// If the field does not exist, it is set to 0 before performing the operation.
    /// The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.
    /// </summary>
    /// <param name="hashField">The field in the hash to increment.</param>
    /// <param name="value">The amount to increment by.</param>
    /// <returns>The value at field after the increment operation.</returns>
    public Task<double> IncrementAsync(string hashField, double value)
        => RedisDb.HashIncrementAsync(Key, hashField, value);

    /// <summary>
    /// Returns all field names in the hash stored at key.
    /// </summary>
    /// <returns>List of fields in the hash, or an empty list when key does not exist.</returns>
    public Task<RedisValue[]> KeysAsync()
        => RedisDb.HashKeysAsync(Key);

    /// <summary>
    /// Returns the number of fields contained in the hash stored at key.
    /// </summary>
    /// <returns>The number of fields in the hash, or 0 when key does not exist.</returns>
    public Task<long> LengthAsync()
        => RedisDb.HashLengthAsync(Key);

    /// <summary>
    /// Incrementally iterate over a hash.
    /// </summary>
    /// <param name="pattern">The pattern of keys to get entries for.</param>
    /// <param name="batchSize">The page size to iterate by.</param>
    /// <returns>Yields all elements of the hash matching the pattern.</returns>
    public IAsyncEnumerable<HashEntry> ScanAsync(string pattern, int batchSize = 250)
    => RedisDb.HashScanAsync(Key, pattern, batchSize);

    /// <summary>
    /// Sets the specified fields to their respective values in the hash stored at key.
    /// This command overwrites any specified fields that already exist in the hash, leaving other unspecified fields untouched.
    /// If key does not exist, a new key holding a hash is created.
    /// </summary>
    /// <param name="hashFields">The entries to set in the hash.</param>
    public Task SetAsync(HashEntry[] hashFields)
        => RedisDb.HashSetAsync(Key, hashFields);

    /// <summary>
    /// Sets field in the hash stored at key to value.
    /// If key does not exist, a new key holding a hash is created. If field already exists in the hash, it is overwritten.
    /// </summary>
    /// <param name="hashField">The field to set in the hash.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="when">Which conditions under which to set the field value (defaults to always).</param>
    /// <returns><see langword="true"/> if field is a new field in the hash and value was set, <see langword="false"/> if field already exists in the hash and the value was updated.</returns>
    public Task<bool> SetAsync(string hashField, RedisValue value, When when = When.Always)
        => RedisDb.HashSetAsync(Key, hashField, value, when);

    /// <summary>
    /// Returns the string length of the value associated with field in the hash stored at key.
    /// </summary>
    /// <param name="hashField">The field containing the string.</param>
    /// <returns>the length of the string at field, or 0 when key does not exist.</returns>
    public Task<long> StringLengthAsync(string hashField)
        => RedisDb.HashStringLengthAsync(Key, hashField);

    /// <summary>
    /// Returns all values in the hash stored at key.
    /// </summary>
    /// <returns>List of values in the hash, or an empty list when key does not exist.</returns>
    public Task<RedisValue[]> ValuesAsync()
        => RedisDb.HashValuesAsync(Key);
}
