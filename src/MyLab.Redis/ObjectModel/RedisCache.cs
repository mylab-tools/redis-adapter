using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MyLab.Redis.ObjectModel
{
    /// <summary>
    /// Caches an object in Redis
    /// </summary>
    public class RedisCache
    {
        private readonly IDatabaseAsync _redisDb;
        private readonly int _redisDbIndex;
        private readonly string _baseKey;

        /// <summary>
        /// Default cache item TTL
        /// </summary>
        /// <remarks>1 hour by default</remarks>
        public TimeSpan DefaultExpiry { get; set; } = TimeSpan.FromHours(1);

        /// <summary>
        /// Initializes a new instance of <see cref="RedisCache"/>
        /// </summary>
        public RedisCache(RedisDbLink redisDb, string baseKey)
        {
            _baseKey = baseKey ?? throw new ArgumentNullException(nameof(baseKey));

            if (redisDb == null)
                throw new ArgumentNullException(nameof(redisDb));
            _redisDb = redisDb.Object;
            _redisDbIndex = redisDb.Index;
        }

        /// <summary>
        /// Retrieve object from cache by id or create new and add into cache
        /// </summary>
        /// <typeparam name="T">cached object type</typeparam>
        /// <param name="id">identifier</param>
        /// <param name="creator">function to create new object when cache is missed</param>
        /// <param name="newItemExpiry">cache item expiry for new item</param>
        /// <returns>cached object</returns>
        public Task<T> FetchAsync<T>(string id, Func<T> creator, TimeSpan? newItemExpiry = null)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (creator == null) throw new ArgumentNullException(nameof(creator));

            return InnerFetchAsync(id, creator, newItemExpiry);
        }

        /// <summary>
        /// Updates cache item expiry
        /// </summary>
        public async Task UpdateExpiryAsync(string itemId, TimeSpan newExpiry)
        {
            var key = GetKey(itemId);
            await key.ExpireAsync(newExpiry);
        }

        /// <summary>
        /// Removes item from cache
        /// </summary>
        public async Task RemoveAsync(string itemId)
        {
            var key = GetKey(itemId);
            await key.DeleteAsync();
        }

        public async Task<bool> IsCountLessThen(int value)
        {
            if (value <= 0)
                return await Task.FromResult(false);

            var ep = await _redisDb.IdentifyEndpointAsync();
            var server = _redisDb.Multiplexer.GetServer(ep);

            var found = server.KeysAsync(_redisDbIndex, _baseKey + ":*", value);
            var foundCount = await found.CountAsync();

            return foundCount < value;
        }


        public async Task<int> Count()
        {
            var ep = await _redisDb.IdentifyEndpointAsync();
            var server = _redisDb.Multiplexer.GetServer(ep);

            return await server.KeysAsync(_redisDbIndex, _baseKey + ":*").CountAsync();
        }

        private async Task<T> InnerFetchAsync<T>(string itemId, Func<T> creator, TimeSpan? newItemExpiry)
        {
            var key = GetKey(itemId);
            var keyValue = await key.GetAsync();

            T res;

            if (!keyValue.IsNull)
            {
                res = JsonConvert.DeserializeObject<T>(keyValue.ToString());
            }
            else
            {
                var newObject = creator();
                var newKeyValue = JsonConvert.SerializeObject(newObject);

                if (!await key.SetAsync(newKeyValue))
                    throw new InvalidOperationException("Can't save new item in cache");
                if (!await key.ExpireAsync(newItemExpiry ?? DefaultExpiry))
                    throw new InvalidOperationException("Can't set expiry for new item in cache");

                res = newObject;
            }

            return res;
        }

        StringRedisKey GetKey(string itemId)
        {
            return new StringRedisKey(_redisDb, _baseKey + ":" + itemId);
        }
    }
}
