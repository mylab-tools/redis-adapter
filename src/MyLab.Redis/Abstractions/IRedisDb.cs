using MyLab.Redis.Keys;
using MyLab.Redis.Scripting;

namespace MyLab.Redis.Abstractions;

public interface IRedisDb
{
    IRedisScriptTools Script();
    StringRedisKey String(string key);
    SortedSetRedisKey SortedSet(string key);
    SetRedisKey Set(string key);
    ListRedisKey List(string key);
    HashRedisKey Hash(string key);
}