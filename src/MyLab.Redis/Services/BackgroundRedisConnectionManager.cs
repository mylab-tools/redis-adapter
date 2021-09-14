using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyLab.Log.Dsl;
using StackExchange.Redis;

namespace MyLab.Redis.Services
{
    class BackgroundRedisConnectionManager : IBackgroundRedisConnectionManager, IDisposable
    {
        private IConnectionMultiplexer _connection;
        private readonly RedisConnector _connector;
        private readonly IDslLogger _log;
        private TimeSpan _retryDelay;
        private bool _isDisposing;

        public BackgroundRedisConnectionManager(IOptions<RedisOptions> options, ILogger<BackgroundRedisConnectionManager> logger = null)
            : this(options.Value, logger)
        {

        }

        public BackgroundRedisConnectionManager(RedisOptions options, ILogger<BackgroundRedisConnectionManager> logger = null)
        {
            _retryDelay = TimeSpan.FromSeconds(options.BackgroundRetryPeriodSec);
            _log = logger?.Dsl();
            _connector = new RedisConnector(options)
            {
                Log = _log
            };
        }

        public event EventHandler Connected;

        public IConnectionMultiplexer ProvideConnection()
        {
            if (_connection == null || !_connection.IsConnected)
                return null;

            return _connection;
        }

        public async Task ConnectAsync()
        {
            bool hasError = false;
            do
            {
                if (hasError)
                {
                    await Task.Delay(_retryDelay);
                    _log?.Action("Connection retrying")
                        .Write();

                }

                try
                {
                    _connection = await _connector.ConnectAsync();
                }
                catch (Exception e)
                {
                    hasError = true;

                    _log?.Error("Initial connection error", e)
                        .Write();
                }

            } while (hasError);

            
            _connection.ConnectionFailed += ConnectionFailed;

            OnConnected();
        }

        public void Dispose()
        {
            _isDisposing = true;
            _connection?.Dispose();
        }

        private void ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            var c = _connection;
            _connection = null;
            c.ConnectionFailed -= ConnectionFailed;
            c.Dispose();

            _log?.Error("Redis connection failed", e.Exception)
                .AndFactIs("failure-type", e.FailureType)
                .Write();

            if(!_isDisposing)
                Connect();
        }

        void Connect()
        {
            bool hasError = false;
            do
            {
                try
                {
                    Thread.Sleep(_retryDelay);
                    _log?.Action("Connection retrying")
                        .Write();
                    _connection = _connector.Connect();
                    _log?.Action("Connection established")
                        .Write();
                }
                catch (Exception e)
                {
                    hasError = true;

                    _log?.Error("Retry connection error", e)
                        .Write();
                }

            } while (hasError);

            _connection.ConnectionFailed += ConnectionFailed;

            OnConnected();
        }

        protected virtual void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }
    }
}