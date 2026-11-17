using MyLab.Redis.Options;

namespace MyLab.Redis.Caching;

/// <summary>
/// A named cache stored in Redis.
/// </summary>
public interface IRedisCache
{
    /// <summary>
    /// Cache identifier from configuration.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Returns the cache entry with the specified identifier.
    /// </summary>
    /// <param name="itemId">Identifier of the cached item.</param>
    IRedisCacheItem Item(string itemId);

    /// <summary>
    /// Counts keys whose names start with <see cref="CacheOptions.BaseKey"/>.
    /// </summary>
    Task<long> GetItemsCountAsync();
}