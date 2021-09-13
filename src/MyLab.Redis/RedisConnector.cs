using System.IO;
using System.Text;
using System.Threading.Tasks;
using MyLab.Log.Dsl;
using StackExchange.Redis;

namespace MyLab.Redis
{
    class RedisConnector
    {
        private readonly ConfigurationOptions _connectionOptions;

        public IDslLogger Log { get; set; }

        public RedisConnector(RedisOptions redisOptions)
        {
            _connectionOptions = new RedisConfigurationOptionsBuilder(redisOptions).Build();
        }

        public async Task<IConnectionMultiplexer> ConnectAsync()
        {
            var sb = new StringBuilder();
            var textWriter = new StringWriter(sb);

            LogStartConnection();

            var connection = await ConnectionMultiplexer.ConnectAsync(_connectionOptions, textWriter);
            
            LogConnected(sb);

            return connection;
        }

        public IConnectionMultiplexer Connect()
        {
            var sb = new StringBuilder();
            var textWriter = new StringWriter(sb);

            LogStartConnection();

            var connection = ConnectionMultiplexer.Connect(_connectionOptions, textWriter);

            LogConnected(sb);

            return connection;
        }

        void LogStartConnection()
        {
            Log?.Action("Try to connect to Redis")
                .AndFactIs("opts", _connectionOptions.ToString())
                .Write();
        }

        void LogConnected(StringBuilder logBuilder)
        {
            Log?.Action("Redis connected")
                .AndFactIs("logs", logBuilder.ToString())
                .Write();
        }
    }
}
