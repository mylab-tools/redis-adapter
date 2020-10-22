using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis
{
    /// <summary>
    /// Provides Redis server tools
    /// </summary>
    public class RedisServerToolsProvider
    {
        private readonly IServer _redisServer;

        public RedisServerToolsProvider(IServer redisServer)
        {
            _redisServer = redisServer;
        }

        /// <summary>
        /// This command is often used to test if a connection is still alive, or to measure latency.
        /// </summary>
        /// <returns>The observed latency.</returns>
        public async Task<TimeSpan> PingAsync()
        {
            return await _redisServer.PingAsync();
        }
    }
}