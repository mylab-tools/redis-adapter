using System.Net;

namespace MyLab.Redis.Services
{
    /// <summary>
    /// Provides Redis interaction features
    /// </summary>
    public interface IRedisService
    {
        /// <summary>
        /// Gets DB tools provider for default database
        /// </summary>
        RedisDbToolsProvider Db();

        /// <summary>
        /// Gets DB tools provider for specified database
        /// </summary>
        RedisDbToolsProvider Db(int dbIndex);

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
