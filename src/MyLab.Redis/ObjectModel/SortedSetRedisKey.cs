using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.ObjectModel
{
    /// <summary>
    /// Represent Redis SORTEDSET
    /// </summary>
    public class SortedSetRedisKey : SortedRedisKey
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SortedSetRedisKey"/>
        /// </summary>
        public SortedSetRedisKey(IDatabaseAsync redisDb, string keyName)
            :base(redisDb, keyName)
        {
            
        }

        /// <summary>
        /// Adds the specified member with the specified score to the sorted set stored at key. If the specified member is already a member of the sorted set, the score is updated and the element reinserted at the right position to ensure the correct ordering.
        /// </summary>
        /// <param name="member">The member to add to the sorted set.</param>
        /// <param name="score">The score for the member to add to the sorted set.</param>
        /// <returns>True if the value was added, False if it already existed (the score is still updated)</returns>
        /// <remarks>https://redis.io/commands/zadd</remarks>
        public Task<bool> AddAsync(RedisValue member, double score)
        {
            return RedisDb.SortedSetAddAsync(KeyName, member, score);
        }

        /// <summary>
        /// Adds all the specified members with the specified scores to the sorted set stored at key. If a specified member is already a member of the sorted set, the score is updated and the element reinserted at the right position to ensure the correct ordering.
        /// </summary>
        /// <param name="values">The members and values to add to the sorted set.</param>
        /// <returns>The number of elements added to the sorted sets, not including elements already existing for which the score was updated.</returns>
        /// <remarks>https://redis.io/commands/zadd</remarks>
        public Task<long> AddAsync(SortedSetEntry[] values)
        {
            return RedisDb.SortedSetAddAsync(KeyName, values);
        }

        /// <summary>
        /// Decrements the score of member in the sorted set stored at key by decrement. If member does not exist in the sorted set, it is added with -decrement as its score (as if its previous score was 0.0).
        /// </summary>
        /// <param name="member">The member to decrement.</param>
        /// <param name="value">The amount to decrement by.</param>
        /// <returns>The new score of member.</returns>
        /// <remarks>https://redis.io/commands/zincrby</remarks>
        public Task<double> DecrementAsync(RedisValue member, double value = 1)
        {
            return RedisDb.SortedSetDecrementAsync(KeyName, member, value);
        }

        /// <summary>
        /// Increments the score of member in the sorted set stored at key by increment. If member does not exist in the sorted set, it is added with increment as its score (as if its previous score was 0.0).
        /// </summary>
        /// <param name="member">The member to increment.</param>
        /// <param name="value">The amount to increment by.</param>
        /// <returns>The new score of member.</returns>
        /// <remarks>https://redis.io/commands/zincrby</remarks>
        public Task<double> IncrementAsync(RedisValue member, double value = 1)
        {
            return RedisDb.SortedSetIncrementAsync(KeyName, member, value);
        }

        /// <summary>
        /// Returns the sorted set cardinality (number of elements) of the sorted set stored at key.
        /// </summary>
        /// <param name="min">The min score to filter by (defaults to negative infinity).</param>
        /// <param name="max">The max score to filter by (defaults to positive infinity).</param>
        /// <param name="exclude">Whether to exclude <paramref name="min"/> and <paramref name="max"/> from the range check (defaults to both inclusive).</param>
        /// <returns>The cardinality (number of elements) of the sorted set, or 0 if key does not exist.</returns>
        /// <remarks>https://redis.io/commands/zcard</remarks>
        public Task<long> LengthAsync(double min = double.NegativeInfinity,
            double max = double.PositiveInfinity, Exclude exclude = Exclude.None)
        {
            return RedisDb.SortedSetLengthAsync(KeyName, min, max, exclude);
        }

        /// <summary>
        /// When all the elements in a sorted set are inserted with the same score, in order to force lexicographical ordering, this command returns the number of elements in the sorted set at key with a value between min and max.
        /// </summary>
        /// <param name="min">The min value to filter by.</param>
        /// <param name="max">The max value to filter by.</param>
        /// <param name="exclude">Whether to exclude <paramref name="min"/> and <paramref name="max"/> from the range check (defaults to both inclusive).</param>
        /// <returns>The number of elements in the specified score range.</returns>
        /// <remarks>https://redis.io/commands/zlexcount</remarks>
        public Task<long> LengthByValueAsync(RedisValue min, RedisValue max,
            Exclude exclude = Exclude.None)
        {
            return RedisDb.SortedSetLengthByValueAsync(KeyName, min, max, exclude);
        }

        /// <summary>
        /// Returns the specified range of elements in the sorted set stored at key. By default the elements are considered to be ordered from the lowest to the highest score. Lexicographical order is used for elements with equal score.
        /// Both start and stop are zero-based indexes, where 0 is the first element, 1 is the next element and so on. They can also be negative numbers indicating offsets from the end of the sorted set, with -1 being the last element of the sorted set, -2 the penultimate element and so on.
        /// </summary>
        /// <param name="start">The start index to get.</param>
        /// <param name="stop">The stop index to get.</param>
        /// <param name="order">The order to sort by (defaults to ascending).</param>
        /// <returns>List of elements in the specified range.</returns>
        /// <remarks>https://redis.io/commands/zrange</remarks>
        /// <remarks>https://redis.io/commands/zrevrange</remarks>
        public Task<RedisValue[]> RangeByRankAsync(long start = 0, long stop = -1,
            Order order = Order.Ascending)
        {
            return RedisDb.SortedSetRangeByRankAsync(KeyName, start, stop, order);
        }

        /// <summary>
        /// Returns the specified range of elements in the sorted set stored at key. By default the elements are considered to be ordered from the lowest to the highest score. Lexicographical order is used for elements with equal score.
        /// Both start and stop are zero-based indexes, where 0 is the first element, 1 is the next element and so on. They can also be negative numbers indicating offsets from the end of the sorted set, with -1 being the last element of the sorted set, -2 the penultimate element and so on.
        /// </summary>
        /// <param name="start">The start index to get.</param>
        /// <param name="stop">The stop index to get.</param>
        /// <param name="order">The order to sort by (defaults to ascending).</param>
        /// <returns>List of elements in the specified range.</returns>
        /// <remarks>https://redis.io/commands/zrange</remarks>
        /// <remarks>https://redis.io/commands/zrevrange</remarks>
        public Task<SortedSetEntry[]> RangeByRankWithScoresAsync(long start = 0, long stop = -1,
            Order order = Order.Ascending)
        {
            return RedisDb.SortedSetRangeByRankWithScoresAsync(KeyName, start, stop, order);
        }

        /// <summary>
        /// Returns the specified range of elements in the sorted set stored at key. By default the elements are considered to be ordered from the lowest to the highest score. Lexicographical order is used for elements with equal score.
        /// Start and stop are used to specify the min and max range for score values. Similar to other range methods the values are inclusive.
        /// </summary>
        /// <param name="start">The minimum score to filter by.</param>
        /// <param name="stop">The maximum score to filter by.</param>
        /// <param name="exclude">Which of <paramref name="start"/> and <paramref name="stop"/> to exclude (defaults to both inclusive).</param>
        /// <param name="order">The order to sort by (defaults to ascending).</param>
        /// <param name="skip">How many items to skip.</param>
        /// <param name="take">How many items to take.</param>
        /// <returns>List of elements in the specified score range.</returns>
        /// <remarks>https://redis.io/commands/zrangebyscore</remarks>
        /// <remarks>https://redis.io/commands/zrevrangebyscore</remarks>
        public Task<RedisValue[]> RangeByScoreAsync(
            double start = double.NegativeInfinity,
            double stop = double.PositiveInfinity,
            Exclude exclude = Exclude.None,
            Order order = Order.Ascending,
            long skip = 0,
            long take = -1)
        {
            return RedisDb.SortedSetRangeByScoreAsync(KeyName, start, stop, exclude, order, skip, take);
        }

        /// <summary>
        /// Returns the specified range of elements in the sorted set stored at key. By default the elements are considered to be ordered from the lowest to the highest score. Lexicographical order is used for elements with equal score.
        /// Start and stop are used to specify the min and max range for score values. Similar to other range methods the values are inclusive.
        /// </summary>
        /// <param name="start">The minimum score to filter by.</param>
        /// <param name="stop">The maximum score to filter by.</param>
        /// <param name="exclude">Which of <paramref name="start"/> and <paramref name="stop"/> to exclude (defaults to both inclusive).</param>
        /// <param name="order">The order to sort by (defaults to ascending).</param>
        /// <param name="skip">How many items to skip.</param>
        /// <param name="take">How many items to take.</param>
        /// <returns>List of elements in the specified score range.</returns>
        /// <remarks>https://redis.io/commands/zrangebyscore</remarks>
        /// <remarks>https://redis.io/commands/zrevrangebyscore</remarks>
        public Task<SortedSetEntry[]> RangeByScoreWithScoresAsync(
            double start = double.NegativeInfinity,
            double stop = double.PositiveInfinity,
            Exclude exclude = Exclude.None,
            Order order = Order.Ascending,
            long skip = 0,
            long take = -1)
        {
            return RedisDb.SortedSetRangeByScoreWithScoresAsync(KeyName, start, stop, exclude, order, skip, take);
        }

        /// <summary>
        /// When all the elements in a sorted set are inserted with the same score, in order to force lexicographical ordering, this command returns all the elements in the sorted set at key with a value between min and max.
        /// </summary>
        /// <param name="min">The min value to filter by.</param>
        /// <param name="max">The max value to filter by.</param>
        /// <param name="exclude">Which of <paramref name="min"/> and <paramref name="max"/> to exclude (defaults to both inclusive).</param>
        /// <param name="skip">How many items to skip.</param>
        /// <param name="take">How many items to take.</param>
        /// <remarks>https://redis.io/commands/zrangebylex</remarks>
        /// <returns>list of elements in the specified score range.</returns>
        public Task<RedisValue[]> RangeByValueAsync(
            RedisValue min,
            RedisValue max,
            Exclude exclude,
            long skip,
            long take = -1) // defaults removed to avoid ambiguity with overload with order
        {
            return RedisDb.SortedSetRangeByValueAsync(KeyName, min, max, exclude, skip, take);
        }

        /// <summary>
        /// When all the elements in a sorted set are inserted with the same score, in order to force lexicographical ordering, this command returns all the elements in the sorted set at key with a value between min and max.
        /// </summary>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="min">The min value to filter by.</param>
        /// <param name="max">The max value to filter by.</param>
        /// <param name="exclude">Which of <paramref name="min"/> and <paramref name="max"/> to exclude (defaults to both inclusive).</param>
        /// <param name="order">Whether to order the data ascending or descending</param>
        /// <param name="skip">How many items to skip.</param>
        /// <param name="take">How many items to take.</param>
        /// <param name="flags">The flags to use for this operation.</param>
        /// <remarks>https://redis.io/commands/zrangebylex</remarks>
        /// <remarks>https://redis.io/commands/zrevrangebylex</remarks>
        /// <returns>list of elements in the specified score range.</returns>
        public Task<RedisValue[]> SortedSetRangeByValueAsync(
            RedisValue min = default(RedisValue),
            RedisValue max = default(RedisValue),
            Exclude exclude = Exclude.None,
            Order order = Order.Ascending,
            long skip = 0,
            long take = -1)
        {
            return RedisDb.SortedSetRangeByValueAsync(KeyName, min, max, exclude, order, skip, take);
        }

        /// <summary>
        /// Returns the rank of member in the sorted set stored at key, by default with the scores ordered from low to high. The rank (or index) is 0-based, which means that the member with the lowest score has rank 0.
        /// </summary>
        /// <param name="member">The member to get the rank of.</param>
        /// <param name="order">The order to sort by (defaults to ascending).</param>
        /// <returns>If member exists in the sorted set, the rank of member; If member does not exist in the sorted set or key does not exist, null</returns>
        /// <remarks>https://redis.io/commands/zrank</remarks>
        /// <remarks>https://redis.io/commands/zrevrank</remarks>
        public Task<long?> SortedSetRankAsync(RedisValue member, Order order = Order.Ascending)
        {
            return RedisDb.SortedSetRankAsync(KeyName, member, order);
        }

        /// <summary>
        /// Removes the specified member from the sorted set stored at key. Non existing members are ignored.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        /// <returns>True if the member existed in the sorted set and was removed; False otherwise.</returns>
        /// <remarks>https://redis.io/commands/zrem</remarks>
        public Task<bool> RemoveMemberAsync(RedisValue member)
        {
            return RedisDb.SortedSetRemoveAsync(KeyName, member);
        }

        /// <summary>
        /// Removes the specified members from the sorted set stored at key. Non existing members are ignored.
        /// </summary>
        /// <param name="members">The members to remove.</param>
        /// <returns>The number of members removed from the sorted set, not including non existing members.</returns>
        /// <remarks>https://redis.io/commands/zrem</remarks>
        public Task<long> RemoveMembersAsync(RedisValue[] members)
        {
            return RedisDb.SortedSetRemoveAsync(KeyName, members);
        }

        /// <summary>
        /// Removes all elements in the sorted set stored at key with rank between start and stop. Both start and stop are 0 -based indexes with 0 being the element with the lowest score. These indexes can be negative numbers, where they indicate offsets starting at the element with the highest score. For example: -1 is the element with the highest score, -2 the element with the second highest score and so forth.
        /// </summary>
        /// <param name="start">The minimum rank to remove.</param>
        /// <param name="stop">The maximum rank to remove.</param>
        /// <returns>The number of elements removed.</returns>
        /// <remarks>https://redis.io/commands/zremrangebyrank</remarks>
        public Task<long> RemoveRangeByRankAsync(long start, long stop)
        {
            return RedisDb.SortedSetRemoveRangeByRankAsync(KeyName, start, stop);
        }

        /// <summary>
        /// Removes all elements in the sorted set stored at key with a score between min and max (inclusive by default).
        /// </summary>
        /// <param name="start">The minimum score to remove.</param>
        /// <param name="stop">The maximum score to remove.</param>
        /// <param name="exclude">Which of <paramref name="start"/> and <paramref name="stop"/> to exclude (defaults to both inclusive).</param>
        /// <returns>The number of elements removed.</returns>
        /// <remarks>https://redis.io/commands/zremrangebyscore</remarks>
        public Task<long> RemoveRangeByScoreAsync(double start, double stop,
            Exclude exclude = Exclude.None)
        {
            return RedisDb.SortedSetRemoveRangeByScoreAsync(KeyName, start, stop, exclude);
        }

        /// <summary>
        /// When all the elements in a sorted set are inserted with the same score, in order to force lexicographical ordering, this command removes all elements in the sorted set stored at key between the lexicographical range specified by min and max.
        /// </summary>
        /// <param name="min">The minimum value to remove.</param>
        /// <param name="max">The maximum value to remove.</param>
        /// <param name="exclude">Which of <paramref name="min"/> and <paramref name="max"/> to exclude (defaults to both inclusive).</param>
        /// <returns>the number of elements removed.</returns>
        /// <remarks>https://redis.io/commands/zremrangebylex</remarks>
        public Task<long> RemoveRangeByValueAsync(RedisValue min, RedisValue max,
            Exclude exclude = Exclude.None)
        {
            return RedisDb.SortedSetRemoveRangeByValueAsync(KeyName, min, max, exclude);
        }

        /// <summary>
        /// The ZSCAN command is used to incrementally iterate over a sorted set
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="pageSize">The page size to iterate by.</param>
        /// <param name="cursor">The cursor position to start at.</param>
        /// <param name="pageOffset">The page offset to start at.</param>
        /// <returns>Yields all matching elements of the sorted set.</returns>
        /// <remarks>https://redis.io/commands/zscan</remarks>
        public IAsyncEnumerable<SortedSetEntry> ScanAsync(string pattern, int pageSize = 250,
            long cursor = 0, int pageOffset = 0)
        {
            return RedisDb.SortedSetScanAsync(KeyName, pattern, pageSize, cursor, pageOffset);
        }

        /// <summary>
        /// Returns the score of member in the sorted set at key; If member does not exist in the sorted set, or key does not exist, nil is returned.
        /// </summary>
        /// <param name="member">The member to get a score for.</param>
        /// <returns>The score of the member.</returns>
        /// <remarks>https://redis.io/commands/zscore</remarks>
        public Task<double?> ScoreAsync(RedisValue member)
        {
            return RedisDb.SortedSetScoreAsync(KeyName, member);
        }

        /// <summary>
        /// Removes and returns the first element from the sorted set stored at key, by default with the scores ordered from low to high.
        /// </summary>
        /// <param name="order">The order to sort by (defaults to ascending).</param>
        /// <returns>The removed element, or nil when key does not exist.</returns>
        /// <remarks>https://redis.io/commands/zpopmin</remarks>
        /// <remarks>https://redis.io/commands/zpopmax</remarks>
        public Task<SortedSetEntry?> PopAsync(Order order = Order.Ascending)
        {
            return RedisDb.SortedSetPopAsync(KeyName, order);
        }

        /// <summary>
        /// Removes and returns the specified number of first elements from the sorted set stored at key, by default with the scores ordered from low to high.
        /// </summary>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <param name="order">The order to sort by (defaults to ascending).</param>
        /// <param name="flags">The flags to use for this operation.</param>
        /// <returns>An array of elements, or an empty array when key does not exist.</returns>
        /// <remarks>https://redis.io/commands/zpopmin</remarks>
        /// <remarks>https://redis.io/commands/zpopmax</remarks>
        public Task<SortedSetEntry[]> PopAsync(RedisKey key, long count, Order order = Order.Ascending,
            CommandFlags flags = CommandFlags.None)
        {
            return RedisDb.SortedSetPopAsync(KeyName, count, order);
        }
    }
}
