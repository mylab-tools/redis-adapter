namespace MyLab.Redis.Options;

public class ConfigCacheOptions : CacheOptions
{
    public OverrideCacheOptions[]? Overrides{ get; set; }
}