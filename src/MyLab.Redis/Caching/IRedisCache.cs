using MyLab.Redis.Abstractions;
using MyLab.Redis.Options;

namespace MyLab.Redis.Caching;

public interface IRedisCache
{
    string Name { get; }
    IRedisCacheItem Item(string itemId);
    Task<long> GetItemsCountAsync();
}

class RedisCache(IRedisDb db, CacheOptions opts) : IRedisCache
{
    public string Name { get; } = opts.Name;

    public IRedisCacheItem Item(string itemId)
        => new RedisCacheItem(itemId, db, opts);

    public Task<long> GetItemsCountAsync()
    {
        throw new NotImplementedException();
    }
}