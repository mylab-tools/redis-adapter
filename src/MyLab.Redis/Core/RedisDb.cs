using MyLab.Redis.Abstractions;
using MyLab.Redis.Keys;
using MyLab.Redis.Scripting;
using StackExchange.Redis;

namespace MyLab.Redis.Core;

class RedisDb(IDatabaseAsync db) : IRedisDb
{
    public int Index => db.Database;

    public IRedisScriptTools Script() => new RedisScriptTools(db);

    public StringRedisKey String(string key) => new (key, db);

    public SortedSetRedisKey SortedSet(string key) => new(key, db);

    public SetRedisKey Set(string key) => new(key, db);

    public ListRedisKey List(string key) => new(key, db);

    public HashRedisKey Hash(string key) => new(key, db);

}