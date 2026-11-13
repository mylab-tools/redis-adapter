using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.Connection
{
    /// <summary>
    /// Provides connection to Redis
    /// </summary>
    public interface IRedisConnectionProvider
    {
        /// <summary>
        /// Provides connection
        /// </summary>
        IConnectionMultiplexer Provide();
    }
}