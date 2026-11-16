using StackExchange.Redis;

namespace MyLab.Redis.Caching;

/// <summary>
/// One item stored in a Redis cache.
/// </summary>
public interface IRedisCacheItem
{
    /// <summary>
    /// Identifier of the cached item.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Redis key formed as <c>{BaseKey}:{Id}</c>.
    /// </summary>
    RedisKey Key { get; }

    /// <summary>
    /// Serializes <paramref name="item"/> to JSON and stores it when the item lock is acquired.
    /// </summary>
    /// <typeparam name="T">Type of the cached value.</typeparam>
    /// <param name="item">Value to store.</param>
    /// <returns><see langword="true"/> when the value was stored.</returns>
    Task<bool> SetAsync<T>(T item);

    /// <summary>
    /// Reads the cached value.
    /// </summary>
    /// <typeparam name="T">Type of the cached value.</typeparam>
    /// <returns>A hit with the deserialized value, or a miss with the default value when the key is missing.</returns>
    Task<CacheGetResult<T>> GetAsync<T>();

    /// <summary>
    /// Reads the cached value, or creates and stores it when the key is missing.
    /// </summary>
    /// <typeparam name="T">Type of the cached value.</typeparam>
    /// <param name="logicAsyncItemProvider">Builds the value when the key does not exist.</param>
    /// <returns>The cached or created value. <see cref="CacheGetResult{T}.Hit"/> is <see langword="false"/> when the created value was not stored.</returns>
    Task<CacheGetResult<T>> GetAsync<T>(AsyncItemProvider<T> logicAsyncItemProvider);

    /// <summary>
    /// Deletes the cached key.
    /// </summary>
    /// <returns><see langword="true"/> when the key was removed.</returns>
    Task<bool> InvalidateAsync();

    /// <summary>
    /// Returns the remaining time to live of the cached key.
    /// </summary>
    /// <returns><see cref="TimeSpan.Zero"/> when the key does not exist or has no expiry.</returns>
    Task<TimeSpan> TtlRemainingAsync();
}