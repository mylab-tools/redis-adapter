using System;

namespace MyLab.Redis
{
    /// <summary>
    /// Contains Redis configuration
    /// </summary>
    public class RedisOptions 
    {
        /// <summary>
        /// Connection string
        /// </summary>
        /// <remarks>https://stackexchange.github.io/StackExchange.Redis/Configuration</remarks>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Overrides password from <see cref="ConnectionString"/>
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Retry period in seconds when 'background' connection mode
        /// </summary>
        public int BackgroundRetryPeriodSec { get; set; } = 10;

        /// <summary>
        /// Caching options
        /// </summary>
        public CachingOptions Caching { get; set; }

        /// <summary>
        /// Locking options
        /// </summary>
        public LockingOptions Locking { get; set; }
    }

    /// <summary>
    /// Cache options
    /// </summary>
    public class CacheOptions
    {
        /// <summary>
        /// Cache name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Default expiry for cache items
        /// </summary>
        public string DefaultExpiry { get; set; } = TimeSpan.FromMinutes(1).ToString();
    }

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
        public CacheOptions[] Caches { get; set; }
    }

    /// <summary>
    /// Lock options
    /// </summary>
    public class LockOptions
    {
        /// <summary>
        /// Lock name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Determines key expiry
        /// </summary>
        public string Expiry { get; set; } = TimeSpan.FromMinutes(1).ToString();

        /// <summary>
        /// Determines the timeout for a locking attempt
        /// </summary>
        public string DefaultTimeout { get; set; } = TimeSpan.FromSeconds(5).ToString();

        /// <summary>
        /// Determines a waiting period between locking attempts
        /// </summary>
        public string RetryPeriod { get; set; } = TimeSpan.FromSeconds(1).ToString();
    }

    /// <summary>
    /// Contains licking options
    /// </summary>
    public class LockingOptions
    {
        /// <summary>
        /// Gets Redis-key name prefix
        /// </summary>
        public string KeyPrefix { get; set; } = "redlock";

        /// <summary>
        /// Gets named lock options
        /// </summary>
        public LockOptions[] Locks { get; set; }
    }
}
