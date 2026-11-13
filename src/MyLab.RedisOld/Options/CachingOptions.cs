namespace MyLab.Redis.Options
{
    /// <summary>
    /// Contains caching options
    /// </summary>
    public class CachingOptions
    {
        /// <summary>
        /// Gets Redis-key name prefix
        /// </summary>
        public string KeyPrefix { get; set; } = "cache";

        /// <summary>
        /// Get named cache options
        /// </summary>
        public CacheOptions[]? Caches { get; set; }
    }
}