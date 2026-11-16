namespace MyLab.Redis.Options;

/// <summary>
/// Cache settings from configuration, including per-cache overrides.
/// </summary>
public class ConfigCacheOptions : CacheOptions
{
    /// <summary>
    /// Settings that replace <see cref="CacheOptions"/> for a cache with the same <see cref="OverrideCacheOptions.Id"/>.
    /// </summary>
    public OverrideCacheOptions[]? Overrides { get; set; }
}
