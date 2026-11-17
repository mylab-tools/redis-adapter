namespace MyLab.Redis.Options
{
    /// <summary>
    /// Connection and feature settings for Redis.
    /// </summary>
    public class RedisOptions
    {
        /// <summary>
        /// Default configuration section name.
        /// </summary>
        public const string SectionName = "Redis";

        /// <summary>
        /// When <see langword="true"/>, a missing connection is reported as healthy.
        /// </summary>
        public bool Optional { get; set; } = false;

        /// <summary>
        /// StackExchange.Redis connection string.
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// ACL user name.
        /// </summary>
        public string? User { get; set; }

        /// <summary>
        /// ACL password.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Cache settings. Missing settings use <see cref="CacheOptions"/> defaults.
        /// </summary>
        public ConfigCacheOptions? Cache { get; set; }

        /// <summary>
        /// Distributed lock settings. Missing settings use <see cref="RedlockOptions"/> defaults.
        /// </summary>
        public RedlockOptions? Redlock { get; set; }
    }
}
