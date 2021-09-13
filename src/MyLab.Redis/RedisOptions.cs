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
        /// Retry period in seconds when background connection mode
        /// </summary>
        public int BackgroundRetryPeriodSec { get; set; } = 10;

        /// <summary>
        /// Cache options
        /// </summary>
        public CacheOptions[] Cache { get; set; }
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
        /// Cache key name
        /// </summary>
        public string Key{ get; set; }

        /// <summary>
        /// Default expiry fro cache items
        /// </summary>
        public string DefaultExpiry { get; set; }
    }
}
