using MyLab.Redis.Core;
using StackExchange.Redis;

namespace MyLab.Redis.Abstractions;

public interface IRedisDb
{
    ScanningEnumerable<RedisKey> GetKeys
    (
        long cursor0,
        string? pattern = null,
        int count = 250,
        int offset = 0
    );
}