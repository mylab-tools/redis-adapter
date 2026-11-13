using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.Connection
{
    /// <summary>
    /// Manage Redis connection
    /// </summary>
    public interface IBackgroundRedisConnectionManager
    {
        /// <summary>
        /// Occurred when Redis connected
        /// </summary>
        event EventHandler Connected;

        /// <summary>
        /// Provides established connection
        /// </summary>
        IConnectionMultiplexer ProvideConnection();

        /// <summary>
        /// Initiate connection
        /// </summary>
        Task ConnectAsync();
    }
}
