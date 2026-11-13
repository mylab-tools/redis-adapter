using System;

namespace MyLab.Redis.Connection
{
    /// <summary>
    /// Defines connection strategy
    /// </summary>
    [Obsolete(null, true)]
    public enum RedisConnectionStrategy
    {
        /// <summary>
        /// Default value
        /// </summary>
        Undefined,
        /// <summary>
        /// Connected when request
        /// </summary>
        Lazy,
        /// <summary>
        /// Connect in background thread
        /// </summary>
        Background
    }
}