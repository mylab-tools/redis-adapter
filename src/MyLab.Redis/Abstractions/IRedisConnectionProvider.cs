using StackExchange.Redis;

namespace MyLab.Redis.Abstractions
{
    /// <summary>
    /// Provides the current Redis connection.
    /// </summary>
    public interface IRedisConnectionProvider
    {
        /// <summary>
        /// Current connection, or <see langword="null"/> when Redis is not connected.
        /// </summary>
        IConnectionMultiplexer? Provide();
    }
}
