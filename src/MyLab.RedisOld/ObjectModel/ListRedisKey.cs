using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.ObjectModel
{
    /// <summary>
    /// Represent Redis LIST
    /// </summary>
    public class ListRedisKey : SortedRedisKey
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ListRedisKey"/>
        /// </summary>
        public ListRedisKey(RedisDbProvider dbProvider, string keyName)
            :base(dbProvider, keyName)
        {
            
        }

        /// <summary>
        /// Returns the element at index index in the list stored at key. The index is zero-based, so 0 means the first element, 1 the second element and so on. Negative indices can be used to designate elements starting at the tail of the list. Here, -1 means the last element, -2 means the penultimate and so forth.
        /// </summary>
        /// <param name="index">The index position to ge the value at.</param>
        /// <returns>The requested element, or nil when index is out of range.</returns>
        /// <remarks>https://redis.io/commands/lindex</remarks>
        public Task<RedisValue> GetByIndexAsync(long index)
        {
            return RedisDb.ListGetByIndexAsync(KeyName, index);
        }

        /// <summary>
        /// Inserts value in the list stored at key either before or after the reference value pivot.
        /// When key does not exist, it is considered an empty list and no operation is performed.
        /// </summary>
        /// <param name="pivot">The value to insert after.</param>
        /// <param name="value">The value to insert.</param>
        /// <returns>The length of the list after the insert operation, or -1 when the value pivot was not found.</returns>
        /// <remarks>https://redis.io/commands/linsert</remarks>
        public Task<long> InsertAfterAsync(RedisValue pivot, RedisValue value)
        {
            return RedisDb.ListInsertAfterAsync(KeyName, pivot, value);
        }

        /// <summary>
        /// Inserts value in the list stored at key either before or after the reference value pivot.
        /// When key does not exist, it is considered an empty list and no operation is performed.
        /// </summary>
        /// <param name="pivot">The value to insert before.</param>
        /// <param name="value">The value to insert.</param>
        /// <returns>The length of the list after the insert operation, or -1 when the value pivot was not found.</returns>
        /// <remarks>https://redis.io/commands/linsert</remarks>
        public Task<long> InsertBeforeAsync(RedisValue pivot, RedisValue value)
        {
            return RedisDb.ListInsertBeforeAsync(KeyName, pivot, value);
        }

        /// <summary>
        /// Removes and returns the first element of the list stored at key.
        /// </summary>
        /// <returns>The value of the first element, or nil when key does not exist.</returns>
        /// <remarks>https://redis.io/commands/lpop</remarks>
        public Task<RedisValue> LeftPopAsync()
        {
            return RedisDb.ListLeftPopAsync(KeyName);
        }

        /// <summary>
        /// Insert the specified value at the head of the list stored at key. If key does not exist, it is created as empty list before performing the push operations.
        /// </summary>
        /// <param name="value">The value to add to the head of the list.</param>
        /// <param name="when">Which conditions to add to the list under (defaults to always).</param>
        /// <returns>The length of the list after the push operations.</returns>
        /// <remarks>https://redis.io/commands/lpush</remarks>
        /// <remarks>https://redis.io/commands/lpushx</remarks>
        public Task<long> LeftPushAsync(RedisValue value, When when = When.Always)
        {
            return RedisDb.ListLeftPushAsync(KeyName, value, when);
        }

        /// <summary>
        /// Insert all the specified values at the head of the list stored at key. If key does not exist, it is created as empty list before performing the push operations.
        /// Elements are inserted one after the other to the head of the list, from the leftmost element to the rightmost element. So for instance the command LPUSH mylist a b c will result into a list containing c as first element, b as second element and a as third element.
        /// </summary>
        /// <param name="values">The values to add to the head of the list.</param>
        /// <returns>The length of the list after the push operations.</returns>
        /// <remarks>https://redis.io/commands/lpush</remarks>
        public Task<long> LeftPushAsync(RedisValue[] values)
        {
            return RedisDb.ListLeftPushAsync(KeyName, values);
        }

        /// <summary>
        /// Returns the length of the list stored at key. If key does not exist, it is interpreted as an empty list and 0 is returned. 
        /// </summary>
        /// <returns>The length of the list at key.</returns>
        /// <remarks>https://redis.io/commands/llen</remarks>
        public Task<long> LengthAsync()
        {
            return RedisDb.ListLengthAsync(KeyName);
        }

        /// <summary>
        /// Returns the specified elements of the list stored at key. The offsets start and stop are zero-based indexes, with 0 being the first element of the list (the head of the list), 1 being the next element and so on.
        /// These offsets can also be negative numbers indicating offsets starting at the end of the list.For example, -1 is the last element of the list, -2 the penultimate, and so on.
        /// Note that if you have a list of numbers from 0 to 100, LRANGE list 0 10 will return 11 elements, that is, the rightmost item is included. 
        /// </summary>
        /// <param name="start">The start index of the list.</param>
        /// <param name="stop">The stop index of the list.</param>
        /// <returns>List of elements in the specified range.</returns>
        /// <remarks>https://redis.io/commands/lrange</remarks>
        public Task<RedisValue[]> RangeAsync(long start = 0, long stop = -1)
        {
            return RedisDb.ListRangeAsync(KeyName, start, stop);
        }

        /// <summary>
        /// Removes the first count occurrences of elements equal to value from the list stored at key. The count argument influences the operation in the following ways:
        /// count &gt; 0: Remove elements equal to value moving from head to tail.
        /// count &lt; 0: Remove elements equal to value moving from tail to head.
        /// count = 0: Remove all elements equal to value.
        /// </summary>
        /// <param name="value">The value to remove from the list.</param>
        /// <param name="count">The count behavior (see method summary).</param>
        /// <returns>The number of removed elements.</returns>
        /// <remarks>https://redis.io/commands/lrem</remarks>
        public Task<long> RemoveElementAsync(RedisValue value, long count = 0)
        {
            return RedisDb.ListRemoveAsync(KeyName, value, count);
        }

        /// <summary>
        /// Removes and returns the last element of the list stored at key.
        /// </summary>
        /// <returns>The element being popped.</returns>
        /// <remarks>https://redis.io/commands/rpop</remarks>
        public Task<RedisValue> RightPopAsync()
        {
            return RedisDb.ListRightPopAsync(KeyName);
        }

        /// <summary>
        /// Insert the specified value at the tail of the list stored at key. If key does not exist, it is created as empty list before performing the push operation.
        /// </summary>
        /// <param name="value">The value to add to the tail of the list.</param>
        /// <param name="when">Which conditions to add to the list under.</param>
        /// <returns>The length of the list after the push operation.</returns>
        /// <remarks>https://redis.io/commands/rpush</remarks>
        /// <remarks>https://redis.io/commands/rpushx</remarks>
        public Task<long> RightPushAsync(RedisValue value, When when = When.Always)
        {
            return RedisDb.ListRightPushAsync(KeyName, value, when);
        }

        /// <summary>
        /// Insert all the specified values at the tail of the list stored at key. If key does not exist, it is created as empty list before performing the push operation. 
        /// Elements are inserted one after the other to the tail of the list, from the leftmost element to the rightmost element. So for instance the command RPUSH mylist a b c will result into a list containing a as first element, b as second element and c as third element.
        /// </summary>
        /// <param name="values">The values to add to the tail of the list.</param>
        /// <param name="when">Which conditions to add to the list under.</param>
        /// <returns>The length of the list after the push operation.</returns>
        /// <remarks>https://redis.io/commands/rpush</remarks>
        public Task<long> RightPushAsync(RedisValue[] values, When when = When.Always)
        {
            return RedisDb.ListRightPushAsync(KeyName, values, when);
        }

        /// <summary>
        /// Sets the list element at index to value. For more information on the index argument, see ListGetByIndex. An error is returned for out of range indexes.
        /// </summary>
        /// <param name="index">The index to set the value at.</param>
        /// <param name="value">The values to add to the list.</param>
        /// <remarks>https://redis.io/commands/lset</remarks>
        public Task SetByIndexAsync(long index, RedisValue value)
        {
            return RedisDb.ListSetByIndexAsync(KeyName, index, value);
        }

        /// <summary>
        /// Trim an existing list so that it will contain only the specified range of elements specified. Both start and stop are zero-based indexes, where 0 is the first element of the list (the head), 1 the next element and so on.
        /// For example: LTRIM foobar 0 2 will modify the list stored at foobar so that only the first three elements of the list will remain.
        /// start and end can also be negative numbers indicating offsets from the end of the list, where -1 is the last element of the list, -2 the penultimate element and so on.
        /// </summary>
        /// <param name="start">The start index of the list to trim to.</param>
        /// <param name="stop">The end index of the list to trim to.</param>
        /// <remarks>https://redis.io/commands/ltrim</remarks>
        public Task TrimAsync(long start, long stop)
        {
            return RedisDb.ListTrimAsync(KeyName, start, stop);
        }
    }
}
