using MyLab.Redis.Abstractions;
using MyLab.Redis.Options;

namespace MyLab.Redis.Caching;

public interface IRedisCacheItem
{
    public string Id { get; }
    Task AddAsync<T>(T item);
    Task GetAsync<T>();
    Task<T> GetOrAddAsync<T>(AsyncItemProvider<T> logicAsyncItemProvider);
    Task RemoveAsync();
}

public class RedisCacheItem(string id, IRedisDb db, CacheOptions cacheOpts) : IRedisCacheItem
{
    public string Id { get; }
    public Task AddAsync<T>(T item)
    {
        throw new NotImplementedException();
    }

    public Task GetAsync<T>()
    {
        throw new NotImplementedException();
    }

    public Task<T> GetOrAddAsync<T>(AsyncItemProvider<T> logicAsyncItemProvider)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync()
    {
        throw new NotImplementedException();
    }
}