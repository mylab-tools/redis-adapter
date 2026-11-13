using System;
using System.ComponentModel.DataAnnotations;

namespace MyLab.Redis.Options
{
    /// <summary>
    /// Cache options
    /// </summary>
    public class CacheOptions
    {
        /// <summary>
        /// Cache name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Default expiry for cache items
        /// </summary>
        public string DefaultExpiry { get; set; } = TimeSpan.FromMinutes(1).ToString();
    }
}