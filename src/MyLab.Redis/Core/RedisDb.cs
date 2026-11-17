using MyLab.Redis.Abstractions;
using MyLab.Redis.Keys;
using MyLab.Redis.Scripting;
using StackExchange.Redis;

namespace MyLab.Redis.Core;

class RedisDb(IDatabaseAsync db) : IRedisDb
{
    public int DbIndex => db.Database;

    public IRedisScriptTools Script() => new RedisScriptTools(db);

    public StringRedisKey String(RedisKey key) => new (key, db);

    public SortedSetRedisKey SortedSet(RedisKey key) => new(key, db);

    public SetRedisKey Set(RedisKey key) => new(key, db);

    public ListRedisKey List(RedisKey key) => new(key, db);

    public HashRedisKey Hash(RedisKey key) => new(key, db);

}