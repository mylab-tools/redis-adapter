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
        await using var lease = await locker.TryAcquireOnceAsync();
        if (lease is null)
            return false;

        return await WriteAsync(item);
    }

    public Task<CacheGetResult<T>> GetAsync<T>() => ReadAsync<T>();

    public async Task<CacheGetResult<T>> GetAsync<T>(AsyncItemProvider<T> logicAsyncItemProvider)
    {
        var cached = await ReadAsync<T>();
        if (cached.Hit)
            return cached;

        await using var lease = await locker.TryAcquireOnceAsync();
        if (lease is null)
            return CacheGetResult<T>.Default;

        var created = await logicAsyncItemProvider(Id);
        var saved = await WriteAsync(created);
        return saved 
            ? new CacheGetResult<T>(false, created)
            : CacheGetResult<T>.Default;
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

    async Task<bool> WriteAsync<T>(T item)
    {
        var json = JsonSerializer.Serialize(item);
        return await db.String(Key).SetAsync(json, Expiry);
    }
}