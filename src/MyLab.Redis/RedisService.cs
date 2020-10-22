using Microsoft.Extensions.Options;
using MyLab.Redis.ObjectModel;
using StackExchange.Redis;

namespace MyLab.Redis
{
    class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _connection;

        public RedisService(IOptions<RedisOptions> options)
            : this(options.Value)
        {

        }

        public RedisService(RedisOptions options)
        {
            var cs = new RedisConfigurationOptionsBuilder(options).Build();

            _connection = ConnectionMultiplexer.Connect(cs);
        }

        public StringRedisKey StringKey(string key)
        {
            return new StringRedisKey(_connection.GetDatabase(), key);
        }

        public Int64RedisKey Int64Key(string key)
        {
            return new Int64RedisKey(_connection.GetDatabase(), key);
        }

        public DoubleRedisKey DoubleKey(string key)
        {
            return new DoubleRedisKey(_connection.GetDatabase(), key);
        }
    }
}
