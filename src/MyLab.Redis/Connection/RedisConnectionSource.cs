using System;
using System.Net.Sockets;
using System.Threading;

namespace MyLab.Redis.Connection
{
    /// <summary>
    /// Provides connection to redis
    /// </summary>
    public class RedisConnectionSource
    {
        private readonly ITcpClientProvider _tcpClientProvider;
        private readonly TimeSpan _connectionRequestTimeout;
        private readonly object _sync = new object();

        /// <summary>
        /// Initializes a new instance of <see cref="RedisConnectionSource"/>
        /// </summary>
        public RedisConnectionSource(RedisOptions options)
            : this(
                new DefaultTcpClientProvider(options.Host, options.Port), 
                TimeSpan.FromSeconds(options.ConnectionRequestTimeout))
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RedisConnectionSource"/>
        /// </summary>
        public RedisConnectionSource(ITcpClientProvider tcpClientProvider, TimeSpan connectionRequestTimeout)
        {
            _tcpClientProvider = tcpClientProvider ?? throw new ArgumentNullException(nameof(tcpClientProvider));
            _connectionRequestTimeout = connectionRequestTimeout;
        }

        public IRedisConnection ProvideConnection()
        {
            if (!Monitor.TryEnter(_sync, (int) _connectionRequestTimeout.TotalMilliseconds))
            {
                throw new ConnectionRequestTimeoutException(_connectionRequestTimeout);
            }

            return new DefaultRedisConnection(_tcpClientProvider.Provide(), _sync);
        }
    }
}