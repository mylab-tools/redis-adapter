using System;
using System.Collections.Generic;
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
        /// <remarks>https://redis.io/commands/ping</remarks>
        public async Task<TimeSpan> PingAsync()
        {
            return await _redisServer.PingAsync();
        }

        /// <summary>
        /// Return the same message passed in
        /// </summary>
        /// <param name="message">The message to echo.</param>
        /// <remarks>https://redis.io/commands/echo</remarks>
        public Task<RedisValue> EchoAsync(RedisValue message)
        {
            return _redisServer.EchoAsync(message);
        }

        /// <summary>
        /// Returns all keys matching pattern; the KEYS or SCAN commands will be used based on the server capabilities; note: to resume an iteration via <i>cursor</i>, cast the original enumerable or enumerator to <i>IScanningCursor</i>.
        /// </summary>
        /// <param name="database">The database ID.</param>
        /// <param name="pattern">The pattern to use.</param>
        /// <param name="pageSize">The page size to iterate by.</param>
        /// <param name="cursor">The cursor position to resume at.</param>
        /// <param name="pageOffset">The page offset to start at.</param>
        /// <remarks>Warning: consider KEYS as a command that should only be used in production environments with extreme care.</remarks>
        /// <remarks>https://redis.io/commands/keys</remarks>
        /// <remarks>https://redis.io/commands/scan</remarks>
        public IAsyncEnumerable<RedisKey> KeysAsync(string pattern, int database = -1,
            int pageSize = 250, long cursor = 0,
            int pageOffset = 0)
        {
            return _redisServer.KeysAsync(database, pattern, pageSize, cursor, pageOffset);
        }

    }
}