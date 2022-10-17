using System;
using System.Net;
using Microsoft.Extensions.Options;
using MyLab.Redis.Connection;

namespace MyLab.Redis.Services
{
    class RedisService : IRedisService
    {
        private readonly IRedisConnectionProvider _connectionProvider;
        private readonly RedisOptions _options;

        public RedisService(IRedisConnectionProvider connectionProvider, IOptions<RedisOptions> opt)
            :this(connectionProvider, opt.Value)
        {

        }
        public RedisService(IRedisConnectionProvider connectionProvider, RedisOptions opt)
        {
            _options = opt;
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public RedisDbToolsProvider Db()
        {
            return Db(-1);
        }

        public RedisDbToolsProvider Db(int dbIndex)
        {
            var dbProvider = new RedisDbProvider(_connectionProvider, dbIndex);
            return new RedisDbToolsProvider(
                dbProvider,
                new RedisCacheFactory(
                    new RedisDbLink
                    {
                        Index = dbIndex,
                        Provider = dbProvider
                    },
                    _options)
            );
        }

        public RedisServerToolsProvider Server()
        {
            return Server(null);
        }

        public RedisServerToolsProvider Server(EndPoint endPoint)
        {
            return new RedisServerToolsProvider(new RedisServerProvider(_connectionProvider, endPoint));
        }
    }

    
}
