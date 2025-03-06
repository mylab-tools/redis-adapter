using System;

namespace MyLab.Redis.Options
{
    /// <summary>
    /// Lock options
    /// </summary>
    public class LockOptions
    {
        /// <summary>
        /// Lock name
        /// </summary>
        public string? Name { get; set; }

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
}