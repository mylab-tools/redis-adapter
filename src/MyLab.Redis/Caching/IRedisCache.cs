using MyLab.Redis.Abstractions;
using MyLab.Redis.Locking;
using MyLab.Redis.Options;

namespace MyLab.Redis.Caching;

public interface IRedisCache
{
    string Id { get; }
    IRedisCacheItem Item(string itemId);
    Task<long> GetItemsCountAsync();
}

class RedisCache(
    string id,
    IRedisDb db,
    IRedisServer server,
    Func<string, IRedisLocker> lockFactory,
    CacheOptions opts) : IRedisCache
{
    public string Id => id;

    public IRedisCacheItem Item(string itemId)
        => new RedisCacheItem(itemId, db, lockFactory(itemId), opts);

    public async Task<long> GetItemsCountAsync()
    {
        long count = 0;
        await foreach (var _ in server.GetKeys(opts.BaseKey + "*", opts.DbIndex))
            count++;

        return count;
    }
}