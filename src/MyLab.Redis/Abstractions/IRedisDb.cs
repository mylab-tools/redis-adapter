using MyLab.Redis.Keys;
using MyLab.Redis.Scripting;
using StackExchange.Redis;

namespace MyLab.Redis.Abstractions;

public interface IRedisDb
{
    IRedisScriptTools Script();
    StringRedisKey String(RedisKey key);
    SortedSetRedisKey SortedSet(RedisKey key);
    SetRedisKey Set(RedisKey key);
    ListRedisKey List(RedisKey key);
    HashRedisKey Hash(RedisKey key);
}