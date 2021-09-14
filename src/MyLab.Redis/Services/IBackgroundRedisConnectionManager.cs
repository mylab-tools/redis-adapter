using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.Services
{
    /// <summary>
    /// Manage Redis connection
    /// </summary>
    public interface IRedisConnectionManager
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
