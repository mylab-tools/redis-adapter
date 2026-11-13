using StackExchange.Redis;

namespace MyLab.Redis.Keys;

public class RedisKeyBase
{
    protected readonly IDatabaseAsync _db;
    public string KeyName { get; }

    protected RedisKeyBase(string keyName, IDatabaseAsync redisDb)
    {
        _db = redisDb;
        KeyName = keyName;
    }

    /// <summary>
    /// Removes the specified key. A key is ignored if it does not exist.
    /// If UNLINK is available (Redis 4.0+), it will be used.
    /// </summary>
    public Task DeleteAsync() => _db.KeyDeleteAsync(KeyName);

    /// <summary>
    /// Serialize the value stored at key in a Redis-specific format and return it to the user.
    /// The returned value can be synthesized back into a Redis key using the RESTORE command.
    /// </summary>
    /// <returns>the serialized value.</returns>
    public Task<byte[]?> DumpAsync() => _db.KeyDumpAsync(KeyName);

    /// <summary>
    /// Returns if key exists.
    /// </summary>
    /// <returns>1 if the key exists. 0 if the key does not exist.</returns>
    public Task<bool> ExistsAsync() => _db.KeyExistsAsync(KeyName);

    /// <summary>
    /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted.
    /// A key with an associated timeout is said to be volatile in Redis terminology.
    /// </summary>
    /// <param name="expiry">The timeout to set.</param>
    /// <param name="when">In Redis 7+, we can choose under which condition the expiration will be set using <see cref="ExpireWhen"/>.</param>
    /// <returns><see langword="true"/> if the timeout was set. <see langword="false"/> if key does not exist or the timeout could not be set.</returns>
    public Task<bool> SetExpirationAsync(TimeSpan? expiry, ExpireWhen when = ExpireWhen.Always)
        => _db.KeyExpireAsync(KeyName, expiry, when);

    /// <summary>
    /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted.
    /// A key with an associated timeout is said to be volatile in Redis terminology.
    /// </summary>
    /// <param name="expiry">The timeout to set.</param>
    /// <param name="when">In Redis 7+, we can choose under which condition the expiration will be set using <see cref="ExpireWhen"/>.</param>
    /// <returns><see langword="true"/> if the timeout was set. <see langword="false"/> if key does not exist or the timeout could not be set.</returns>
    public Task<bool> SetExpirationAsync(DateTime? expiry, ExpireWhen when = ExpireWhen.Always)
        => _db.KeyExpireAsync(KeyName, expiry, when);

    /// <summary>
    /// Returns the absolute time at which the given key will expire, if it exists and has an expiration.
    /// </summary>
    /// <returns>The time at which the given key will expire, or <see langword="null"/> if the key does not exist or has no associated expiration time.</returns>
    public async Task<DateTime?> GetExpirationAsync()
    {
        var universalDt = await _db.KeyExpireTimeAsync(KeyName);
        return universalDt?.ToLocalTime();
    }

    /// <summary>
    /// Returns the time since the object stored at the specified key is idle (not requested by read or write operations).
    /// </summary>
    /// <returns>The time since the object stored at the specified key is idle</returns>
    public Task<TimeSpan?> GetIdleTimeAsync() => _db.KeyIdleTimeAsync(KeyName);

    /// <summary>
    /// Remove the existing timeout on key, turning the key from volatile (a key with an expiry set) to persistent (a key that will never expire as no timeout is associated).
    /// </summary>
    /// <returns><see langword="true"/> if the timeout was removed. <see langword="false"/> if key does not exist or does not have an associated timeout.</returns>
    public Task<bool> PersistAsync() => _db.KeyPersistAsync(KeyName);

    /// <summary>
    /// Returns the remaining time to live of a key that has a timeout.
    /// This introspection capability allows a Redis client to check how many seconds a given key will continue to be part of the dataset.
    /// </summary>
    /// <returns>TTL, or nil when key does not exist or does not have a timeout.</returns>
    public Task<TimeSpan?> GetTtlAsync() => _db.KeyTimeToLiveAsync(KeyName);

    /// <summary>
    /// Returns the string representation of the type of the value stored at key.
    /// The different types that can be returned are: string, list, set, zset and hash.
    /// </summary>
    /// <returns>Type of key, or none when key does not exist.</returns>
    public Task<RedisType> GetRedisTypeAsync() => _db.KeyTypeAsync(KeyName);
}