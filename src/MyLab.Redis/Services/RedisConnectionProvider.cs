using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyLab.Log.Dsl;
using StackExchange.Redis;

namespace MyLab.Redis.Services
{
    class RedisConnectionProvider : IRedisConnectionProvider
    {
        private readonly ConfigurationOptions _connectionOptions;
        private IConnectionMultiplexer _connection;
        private readonly object _sync = new object();
        private readonly IDslLogger _log;

        public RedisConnectionProvider(IOptions<RedisOptions> options, ILogger<RedisConnectionProvider> logger = null)
            : this(options.Value, logger)
        {

        }

        public RedisConnectionProvider(RedisOptions options, ILogger<RedisConnectionProvider> logger = null)
        {
            _connectionOptions = new RedisConfigurationOptionsBuilder(options).Build();
            _log = logger?.Dsl();
        }

        public IConnectionMultiplexer Provide()
        {
            lock (_sync)
            {
                if (_connection == null)
                {
                    var sb = new StringBuilder();
                    var textWriter = new StringWriter(sb);

                    _log?.Action("Try to connect to Redis")
                        .AndFactIs("opts", _connectionOptions.ToString())
                        .Write();

                    _connection = ConnectionMultiplexer.Connect(_connectionOptions, textWriter);

                    _log?.Action("Redis connected")
                        .AndFactIs("logs", sb.ToString())
                        .Write();
                }

                return _connection;
            }
        }
    }
}