using MyLab.Redis.Abstractions;
using StackExchange.Redis;

namespace MyLab.Redis.Core;

class RedisDb(IDatabaseAsync db, IServer server) : IRedisDb
{
    public ScanningEnumerable<RedisKey> GetKeys
    (
        long cursor0,
        string? pattern = null,
        int count = 250,
        int offset = 0
    )
    {
        RedisValue redisPattern = pattern ?? default(RedisValue);
        var keyEnumerable = server.KeysAsync
        (
            pattern: redisPattern,
            pageSize: count,
            pageOffset: offset,
            cursor: cursor0,
            database: db.Database
        );

        return new ScanningEnumerable<RedisKey>(keyEnumerable);
    }
}