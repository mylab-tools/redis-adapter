using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.ObjectModel
{
    /// <summary>
    /// Represent Redis HASH
    /// </summary>
    public class HashRedisKey : RedisKeyBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HashRedisKey"/>
        /// </summary>
        public HashRedisKey(IDatabaseAsync redisDb, string keyName)
            :base(redisDb, keyName)
        {
            
        }

        /// <summary>
        /// Decrements the number stored at field in the hash stored at key by decrement. If key does not exist, a new key holding a hash is created. If field does not exist or holds a string that cannot be interpreted as integer, the value is set to 0 before the operation is performed.
        /// </summary>
        /// <param name="hashField">The field in the hash to decrement.</param>
        /// <param name="value">The amount to decrement by.</param>
        /// <returns>The value at field after the decrement operation.</returns>
        /// <remarks>https://redis.io/commands/hincrby</remarks>
        public Task<long> DecrementAsync(string hashField, long value = 1)
        {
            return RedisDb.HashDecrementAsync(KeyName, hashField, value);
        }

        /// <summary>s
        /// Decrement the specified field of an hash stored at key, and representing a floating point number, by the specified decrement. If the field does not exist, it is set to 0 before performing the operation.
        /// </summary>
        /// <param name="hashField">The field in the hash to decrement.</param>
        /// <param name="value">The amount to decrement by.</param>
        /// <returns>The value at field after the decrement operation.</returns>
        /// <remarks>The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.</remarks>
        /// <remarks>https://redis.io/commands/hincrbyfloat</remarks>
        public Task<double> DecrementAsync(string hashField, double value)
        {
            return RedisDb.HashDecrementAsync(KeyName, hashField, value);
        }

        /// <summary>
        /// Removes the specified fields from the hash stored at key. Non-existing fields are ignored. Non-existing keys are treated as empty hashes and this command returns 0.
        /// </summary>
        /// <param name="hashField">The field in the hash to delete.</param>
        /// <remarks>https://redis.io/commands/hdel</remarks>
        public Task DeleteFieldAsync(string hashField)
        {
            return RedisDb.HashDeleteAsync(KeyName, hashField);
        }

        /// <summary>
        /// Removes the specified fields from the hash stored at key. Non-existing fields are ignored. Non-existing keys are treated as empty hashes and this command returns 0.
        /// </summary>
        /// <param name="hashFields">The fields in the hash to delete.</param>
        /// <returns>The number of fields that were removed.</returns>
        /// <remarks>https://redis.io/commands/hdel</remarks>
        public Task<long> DeleteFieldsAsync(string[] hashFields)
        {
            if (hashFields == null) throw new ArgumentNullException(nameof(hashFields));
            return RedisDb.HashDeleteAsync(KeyName, hashFields.Select(f => new RedisValue(f)).ToArray());
        }

        /// <summary>
        /// Returns if field is an existing field in the hash stored at key.
        /// </summary>
        /// <param name="hashField">The field in the hash to check.</param>
        /// <returns>1 if the hash contains field. 0 if the hash does not contain field, or key does not exist.</returns>
        /// <remarks>https://redis.io/commands/hexists</remarks>
        public Task<bool> FieldExistsAsync(string hashField)
        {
            return RedisDb.HashExistsAsync(KeyName, hashField);
        }

        /// <summary>
        /// Returns the value associated with field in the hash stored at key.
        /// </summary>
        /// <param name="hashField">The field in the hash to get.</param>
        /// <returns>The value associated with field, or nil when field is not present in the hash or key does not exist.</returns>
        /// <remarks>https://redis.io/commands/hget</remarks>
        public Task<RedisValue> GetAsync(string hashField)
        {
            return RedisDb.HashGetAsync(KeyName, hashField);
        }

        /// <summary>
        /// Returns the values associated with the specified fields in the hash stored at key.
        /// For every field that does not exist in the hash, a nil value is returned.Because a non-existing keys are treated as empty hashes, running HMGET against a non-existing key will return a list of nil values.
        /// </summary>
        /// <param name="hashFields">The fields in the hash to get.</param>
        /// <returns>List of values associated with the given fields, in the same order as they are requested.</returns>
        /// <remarks>https://redis.io/commands/hmget</remarks>
        public Task<RedisValue[]> GetAsync(string[] hashFields)
        {
            return RedisDb.HashGetAsync(KeyName, hashFields.Select(f => new RedisValue(f)).ToArray());
        }

        /// <summary>
        /// Returns all fields and values of the hash stored at key. 
        /// </summary>
        /// <returns>List of fields and their values stored in the hash, or an empty list when key does not exist.</returns>
        /// <remarks>https://redis.io/commands/hgetall</remarks>
        public Task<HashEntry[]> GetAllAsync()
        {
            return RedisDb.HashGetAllAsync(KeyName);
        }

        /// <summary>
        /// Increments the number stored at field in the hash stored at key by increment. If key does not exist, a new key holding a hash is created. If field does not exist or holds a string that cannot be interpreted as integer, the value is set to 0 before the operation is performed.
        /// </summary>
        /// <param name="hashField">The field in the hash to increment.</param>
        /// <param name="value">The amount to increment by.</param>
        /// <returns>The value at field after the increment operation.</returns>
        /// <remarks>The range of values supported by HINCRBY is limited to 64 bit signed integers.</remarks>
        /// <remarks>https://redis.io/commands/hincrby</remarks>
        public Task<long> IncrementAsync(string hashField, long value = 1)
        {
            return RedisDb.HashIncrementAsync(KeyName, hashField, value);
        }

        /// <summary>
        /// Increment the specified field of an hash stored at key, and representing a floating point number, by the specified increment. If the field does not exist, it is set to 0 before performing the operation.
        /// </summary>
        /// <param name="hashField">The field in the hash to increment.</param>
        /// <param name="value">The amount to increment by.</param>
        /// <returns>The value at field after the increment operation.</returns>
        /// <remarks>The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.</remarks>
        /// <remarks>https://redis.io/commands/hincrbyfloat</remarks>
        public Task<double> IncrementAsync(string hashField, double value)
        {
            return RedisDb.HashIncrementAsync(KeyName, hashField, value);
        }

        /// <summary>
        /// Returns all field names in the hash stored at key.
        /// </summary>
        /// <returns>List of fields in the hash, or an empty list when key does not exist.</returns>
        /// <remarks>https://redis.io/commands/hkeys</remarks>
        public Task<RedisValue[]> KeysAsync()
        {
            return RedisDb.HashKeysAsync(KeyName);
        }

        /// <summary>
        /// Returns the number of fields contained in the hash stored at key.
        /// </summary>
        /// <returns>The number of fields in the hash, or 0 when key does not exist.</returns>
        /// <remarks>https://redis.io/commands/hlen</remarks>
        public Task<long> LengthAsync()
        {
            return RedisDb.HashLengthAsync(KeyName);
        }

        /// <summary>
        /// The HSCAN command is used to incrementally iterate over a hash; note: to resume an iteration via <i>cursor</i>, cast the original enumerable or enumerator to <i>IScanningCursor</i>.
        /// </summary>
        /// <param name="pattern">The pattern of keys to get entries for.</param>
        /// <param name="pageSize">The page size to iterate by.</param>
        /// <param name="cursor">The cursor position to start at.</param>
        /// <param name="pageOffset">The page offset to start at.</param>
        /// <returns>Yields all elements of the hash matching the pattern.</returns>
        /// <remarks>https://redis.io/commands/hscan</remarks>
        public IAsyncEnumerable<HashEntry> ScanAsync(string pattern, int pageSize = 250, long cursor = 0, int pageOffset = 0)
        {
            return RedisDb.HashScanAsync(KeyName, pattern, pageSize, cursor, pageOffset);
        }

        /// <summary>
        /// Sets the specified fields to their respective values in the hash stored at key. This command overwrites any specified fields that already exist in the hash, leaving other unspecified fields untouched. If key does not exist, a new key holding a hash is created.
        /// </summary>
        /// <param name="hashFields">The entries to set in the hash.</param>
        /// <remarks>https://redis.io/commands/hmset</remarks>
        public Task SetAsync(HashEntry[] hashFields)
        {
            return RedisDb.HashSetAsync(KeyName, hashFields);
        }

        /// <summary>
        /// Sets field in the hash stored at key to value. If key does not exist, a new key holding a hash is created. If field already exists in the hash, it is overwritten.
        /// </summary>
        /// <param name="hashField">The field to set in the hash.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="when">Which conditions under which to set the field value (defaults to always).</param>
        /// <remarks>https://redis.io/commands/hset</remarks>
        /// <remarks>https://redis.io/commands/hsetnx</remarks>
        public async Task SetAsync(string hashField, RedisValue value)
        {
            await RedisDb.HashSetAsync(hashField, hashField, value);
        }

        /// <summary>
        /// Returns the string length of the value associated with field in the hash stored at key.
        /// </summary>
        /// <param name="hashField">The field containing the string</param>
        /// <returns>the length of the string at field, or 0 when key does not exist.</returns>
        /// <remarks>https://redis.io/commands/hstrlen</remarks>
        public Task<long> StringLengthAsync(string hashField)
        {
            return RedisDb.HashStringLengthAsync(KeyName, hashField);
        }

        /// <summary>
        /// Returns all values in the hash stored at key.
        /// </summary>
        /// <returns>List of values in the hash, or an empty list when key does not exist.</returns>
        /// <remarks>https://redis.io/commands/hvals</remarks>
        public Task<RedisValue[]> ValuesAsync()
        {
            return RedisDb.HashValuesAsync(KeyName);
        }
    }
}
