namespace MyLab.Redis.Options;

/// <summary>
/// Settings of one Redis cache.
/// </summary>
public class CacheOptions
{
    /// <summary>
    /// Settings used when a value is left unchanged.
    /// </summary>
    public static readonly CacheOptions Default = new();

    /// <summary>
    /// Item lifetime in seconds. <c>0</c> stores the item without expiry.
    /// </summary>
    public int ExpirySec { get; set; } = 0;

    /// <summary>
    /// Database index. A negative value selects the configured default database.
    /// </summary>
    public int DbIndex { get; set; } = -1;

    /// <summary>
    /// Prefix of cache keys. An item key is <c>{BaseKey}:{itemId}</c>.
    /// </summary>
    public string BaseKey { get; set; } = "cache";

    /// <summary>
    /// Lock identifier prefix configured for this cache.
    /// </summary>
    public string LockIdPrefix { get; set; } = "cache";
}
