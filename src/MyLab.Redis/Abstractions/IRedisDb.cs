using MyLab.Redis.Keys;
using MyLab.Redis.Scripting;
using StackExchange.Redis;

namespace MyLab.Redis.Abstractions;

/// <summary>
/// Commands for one Redis database.
/// </summary>
public interface IRedisDb
{
    /// <summary>
    /// Server-side Lua script commands for this database.
    /// </summary>
    IRedisScriptTools Script();

    /// <summary>
    /// String key in this database.
    /// </summary>
    /// <param name="key">Name of the key.</param>
    StringRedisKey String(RedisKey key);

    /// <summary>
    /// Sorted-set key in this database.
    /// </summary>
    /// <param name="key">Name of the key.</param>
    SortedSetRedisKey SortedSet(RedisKey key);

    /// <summary>
    /// Set key in this database.
    /// </summary>
    /// <param name="key">Name of the key.</param>
    SetRedisKey Set(RedisKey key);

    /// <summary>
    /// List key in this database.
    /// </summary>
    /// <param name="key">Name of the key.</param>
    ListRedisKey List(RedisKey key);

    /// <summary>
    /// Hash key in this database.
    /// </summary>
    /// <param name="key">Name of the key.</param>
    HashRedisKey Hash(RedisKey key);
}