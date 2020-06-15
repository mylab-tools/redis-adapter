using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MyLab.Redis.Commands.Connection;
using Nito.AsyncEx;

namespace MyLab.Redis.Connection
{
    /// <summary>
    /// Provides connection to redis
    /// </summary>
    public class RedisConnectionSource : IDisposable
    {
        private readonly ITcpClientProvider _tcpClientProvider;
        private readonly IRedisConnectionFactory _connectionFactory;
        private readonly TimeSpan _connectionRequestTimeout;
        private readonly AsyncLock _sync = new AsyncLock();
        private readonly short _dbIndex;
        private readonly string _password;

        /// <summary>
        /// Initializes a new instance of <see cref="RedisConnectionSource"/>
        /// </summary>
        public RedisConnectionSource(IOptions<RedisOptions> options)
            : this(options.Value)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RedisConnectionSource"/>
        /// </summary>
        public RedisConnectionSource(RedisOptions options)
            : this(
                new DefaultTcpClientProvider(options.Host, options.Port), 
                new DefaultRedisConnectionFactory(options.Encoding != null 
                    ? Encoding.GetEncoding(options.Encoding) 
                    : Encoding.UTF8), 
                TimeSpan.FromSeconds(options.ConnectionRequestTimeout),
                options.DbIndex,
                options.Password)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RedisConnectionSource"/>
        /// </summary>
        public RedisConnectionSource(
            ITcpClientProvider tcpClientProvider,
            IRedisConnectionFactory connectionFactory,
            TimeSpan connectionRequestTimeout,
            short dbIndex,
            string password)
        {
            _tcpClientProvider = tcpClientProvider ?? throw new ArgumentNullException(nameof(tcpClientProvider));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _connectionRequestTimeout = connectionRequestTimeout;

            _dbIndex = dbIndex;
            _password = password;
        }
        
        public async Task<IRedisConnection> ProvideConnectionAsync()
        {
            var cancellationTokenSource = new CancellationTokenSource(_connectionRequestTimeout);

            IDisposable syncDisposer;
            try
            {
                syncDisposer = await _sync.LockAsync(cancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                throw new ConnectionRequestTimeoutException(_connectionRequestTimeout);
            }

            var tcpClient = _tcpClientProvider.Provide(out var isNewConnection);

            var connection = _connectionFactory.Create(tcpClient, syncDisposer);

            if (isNewConnection)
            {
                if (!string.IsNullOrEmpty(_password))
                {
                    var authCmd = new AuthRedisCmd(_password);
                    await authCmd.PerformAsync(connection);
                }

                if (_dbIndex > 0)
                {
                    var switchDbCmd = new SelectDbRedisCmd(_dbIndex);
                    await switchDbCmd.PerformAsync(connection);
                }
            }

            return connection;
        }

        public void Dispose()
        {
            _tcpClientProvider?.Dispose();
        }
    }
}