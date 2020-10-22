using System.Net;

namespace MyLab.Redis
{
    /// <summary>
    /// Provides Redis interaction features
    /// </summary>
    public interface IRedisService
    {
        /// <summary>
        /// Gets DB keys provider
        /// </summary>
        RedisDbKeysProvider Keys();

        /// <summary>
        /// Provides tools for default server
        /// </summary>
        RedisServerToolsProvider Server();
        /// <summary>
        /// Provides tools for server with specified endpoint
        /// </summary>
        RedisServerToolsProvider Server(EndPoint endPoint);
    }
}
