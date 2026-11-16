using System.Text.Json;
using MyLab.Redis.Abstractions;
using MyLab.Redis.Locking;
using MyLab.Redis.Options;
using StackExchange.Redis;

namespace MyLab.Redis.Caching;

class RedisCacheItem(string id, IRedisDb db, IRedisLocker locker, CacheOptions cacheOpts) : IRedisCacheItem
{
    public string Id => id;

    public RedisKey Key { get; } = $"{cacheOpts.BaseKey}:{id}";

    TimeSpan? Expiry => cacheOpts.ExpirySec > 0
        ? TimeSpan.FromSeconds(cacheOpts.ExpirySec)
        : null;

    public async Task<bool> SetAsync<T>(T item)
    {
        var json = JsonSerializer.Serialize(item);

        await using var lease = await locker.TryAcquireOnceAsync();
        if (lease is null)
            return false;

        return await db.String(Key).SetAsync(json, Expiry);
    }

    public Task<CacheGetResult<T>> GetAsync<T>() => ReadAsync<T>();

    public async Task<CacheGetResult<T>> GetAsync<T>(AsyncItemProvider<T> logicAsyncItemProvider)
    {
        var cached = await ReadAsync<T>();
        if (cached.Hit)
            return cached;

        var created = await logicAsyncItemProvider(Id);
        var saved = await SetAsync(created);
        return new CacheGetResult<T>(saved, created);
    }

    public Task<bool> InvalidateAsync() => db.String(Key).DeleteAsync();

    public async Task<TimeSpan> TtlRemainingAsync()
        => await db.String(Key).GetTtlAsync() ?? TimeSpan.Zero;

    async Task<CacheGetResult<T>> ReadAsync<T>()
    {
        var value = await db.String(Key).GetAsync();
        if (value.IsNull)
            return new CacheGetResult<T>(false, default!);

        var item = JsonSerializer.Deserialize<T>(value.ToString());
        return new CacheGetResult<T>(true, item!);
    }
}