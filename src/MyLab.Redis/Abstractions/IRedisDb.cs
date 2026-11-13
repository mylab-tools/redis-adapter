using MyLab.Redis.Core;

namespace MyLab.Redis.Abstractions;

public interface IRedisDb
{
    RedisKeyEnumerable GetKeys
    (
        long cursor0,
        string? pattern = null,
        int count = 250,
        int offset = 0
    );
}