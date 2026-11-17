using MyLab.Redis.Abstractions;
using MyLab.Redis.Locking;
using MyLab.Redis.Options;

namespace MyLab.Redis.Caching;

class RedisCache(
    string id,
    IRedisDb db,
    IRedisServer server,
    Func<string, IRedisLocker> lockFactory,
    CacheOptions opts) : IRedisCache
{
    public string Id => id;

    public IRedisCacheItem Item(string itemId)
        => new RedisCacheItem(itemId, db, lockFactory($"{opts.LockIdPrefix}:{itemId}"), opts);

    public async Task<long> GetItemsCountAsync()
    {
        long count = 0;
        await foreach (var _ in server.GetKeysAsync(opts.BaseKey + "*", opts.DbIndex))
            count++;

        return count;
    }
}