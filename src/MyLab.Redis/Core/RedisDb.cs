using MyLab.Redis.Abstractions;
using MyLab.Redis.Scripting;
using StackExchange.Redis;

namespace MyLab.Redis.Core;

class RedisDb(IDatabaseAsync db) : IRedisDb
{
    public int Index => db.Database;

    public IRedisScriptTools Script() => new RedisScriptTools(db);
}