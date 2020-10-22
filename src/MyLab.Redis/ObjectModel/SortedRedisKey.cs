using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.ObjectModel
{
    /// <summary>
    /// Represent a key with sorting abilities
    /// </summary>
    public class SortedRedisKey : RedisKeyBase
    {
        protected SortedRedisKey(IDatabaseAsync redisDb, string keyName) : base(redisDb, keyName)
        {
        }

        /// <summary>
        /// Sorts a list, set or sorted set (numerically or alphabetically, ascending by default); By default, the elements themselves are compared, but the values can also be
        /// used to perform external key-lookups using the <c>by</c> parameter. By default, the elements themselves are returned, but external key-lookups (one or many) can
        /// be performed instead by specifying the <c>get</c> parameter (note that <c>#</c> specifies the element itself, when used in <c>get</c>).
        /// Referring to the <a href="https://redis.io/commands/sort">redis SORT documentation </a> for examples is recommended. When used in hashes, <c>by</c> and <c>get</c>
        /// can be used to specify fields using <c>-&gt;</c> notation (again, refer to redis documentation).
        /// </summary>
        /// <param name="skip">How many entries to skip on the return.</param>
        /// <param name="take">How many entries to take on the return.</param>
        /// <param name="order">The ascending or descending order (defaults to ascending).</param>
        /// <param name="sortType">The sorting method (defaults to numeric).</param>
        /// <param name="by">The key pattern to sort by, if any. e.g. ExternalKey_* would sort by ExternalKey_{listvalue} as a lookup.</param>
        /// <param name="get">The key pattern to sort by, if any e.g. ExternalKey_* would return the value of ExternalKey_{listvalue} for each entry.</param>
        /// <returns>The sorted elements, or the external values if <c>get</c> is specified.</returns>
        /// <remarks>https://redis.io/commands/sort</remarks>
        public Task<RedisValue[]> SortAsync(long skip = 0, long take = -1, Order order = Order.Ascending,
            SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[] get = null)
        {
            return RedisDb.SortAsync(KeyName, skip, take, order, sortType, by, get));
        }

        /// <summary>
        /// Sorts a list, set or sorted set (numerically or alphabetically, ascending by default); By default, the elements themselves are compared, but the values can also be
        /// used to perform external key-lookups using the <c>by</c> parameter. By default, the elements themselves are returned, but external key-lookups (one or many) can
        /// be performed instead by specifying the <c>get</c> parameter (note that <c>#</c> specifies the element itself, when used in <c>get</c>).
        /// Referring to the <a href="https://redis.io/commands/sort">redis SORT documentation </a> for examples is recommended. When used in hashes, <c>by</c> and <c>get</c>
        /// can be used to specify fields using <c>-&gt;</c> notation (again, refer to redis documentation).
        /// </summary>
        /// <param name="destination">The destination key to store results in.</param>
        /// <param name="skip">How many entries to skip on the return.</param>
        /// <param name="take">How many entries to take on the return.</param>
        /// <param name="order">The ascending or descending order (defaults to ascending).</param>
        /// <param name="sortType">The sorting method (defaults to numeric).</param>
        /// <param name="by">The key pattern to sort by, if any. e.g. ExternalKey_* would sort by ExternalKey_{listvalue} as a lookup.</param>
        /// <param name="get">The key pattern to sort by, if any e.g. ExternalKey_* would return the value of ExternalKey_{listvalue} for each entry.</param>
        /// <returns>The number of elements stored in the new list.</returns>
        /// <remarks>https://redis.io/commands/sort</remarks>
        public Task<long> SortAndStoreAsync(RedisKey destination, long skip = 0, long take = -1,
            Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default,
            RedisValue[] get = null)
        {
            return RedisDb.SortAndStoreAsync(destination, KeyName, skip, take, order, sortType, by, get);
        }
    }
}
