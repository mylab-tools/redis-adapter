namespace MyLab.Redis
{
    /// <summary>
    /// Defines connection strategy
    /// </summary>
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