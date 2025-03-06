namespace MyLab.Redis.Options
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
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Overrides password from <see cref="ConnectionString"/>
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Retry period in seconds when 'background' connection mode
        /// </summary>
        public int BackgroundRetryPeriodSec { get; set; } = 10;

        /// <summary>
        /// Caching options
        /// </summary>
        public CachingOptions? Caching { get; set; }

        /// <summary>
        /// Locking options
        /// </summary>
        public LockingOptions? Locking { get; set; }
    }
}
