using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MyLab.Redis
{
    class RedisService : IRedisService
    {
        private readonly RedisOptions _options;
        private readonly ConnectionMultiplexer _connection;

        public RedisService(IOptions<RedisOptions> options)
            : this(options.Value)
        {

        }

        public RedisService(RedisOptions options)
        {
            _options = options;
            var cs = new RedisConfigurationOptionsBuilder(options).Build();

            _connection = ConnectionMultiplexer.Connect(cs);
        }

        public RedisDbToolsProvider Db()
        {
            var db = _connection.GetDatabase(-1);

            return new RedisDbToolsProvider(
                db,
                new RedisCacheProvider(
                    new RedisDbLink
                    {
                        Index = -1,
                        Object = db
                    },
                    _options)
            );
        }

        public RedisDbToolsProvider Db(int dbIndex)
        {
            var db = _connection.GetDatabase(dbIndex);

            return new RedisDbToolsProvider(
                db,
                new RedisCacheProvider(
                    new RedisDbLink
                    {
                        Index = dbIndex,
                        Object = db
                    },
                    _options)
            );
        }

        public RedisServerToolsProvider Server()
        {
            return new RedisServerToolsProvider(GetServer());
        }

        public RedisServerToolsProvider Server(EndPoint endPoint)
        {
            return new RedisServerToolsProvider(GetServer(endPoint));
        }

        IServer GetServer(EndPoint endpoint = null)
        {
            return _connection.GetServer(
                endpoint ?? 
                _connection.GetEndPoints().First()
                );
        }
    }
}
