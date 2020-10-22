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
        public StringRedisKey(IDatabaseAsync redisDb, string keyName) 
            : base(redisDb, keyName)
        {
        }

        /// <summary>
        /// Set key to hold the string value
        /// </summary>
        public async Task SetValueAsync(string value)
        {
            await PerformAndCheckSuccessful(RedisDb.StringSetAsync(KeyName, value, null, When.Always));
        }

        /// <summary>
        /// Get the value of key
        /// </summary>
        public async Task<string> GetValueAsync()
        {
            var res = await PerformAndCheckSuccessful(RedisDb.StringGetAsync(KeyName));
            return res.ToString();
        }

        /// <summary>
        /// Returns the length of the string value stored at key.
        /// </summary>
        public async Task<long> GetLengthAsync()
        {
            return await PerformAndCheckSuccessful(RedisDb.StringLengthAsync(KeyName));
        }
    }
}