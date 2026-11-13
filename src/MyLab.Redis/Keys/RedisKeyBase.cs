using StackExchange.Redis;

namespace MyLab.Redis.Keys;

public class RedisKeyBase
{
    private readonly IDatabaseAsync _db;
    public string KeyName { get; }

    protected RedisKeyBase(string keyName, IDatabaseAsync redisDb)
    {
        _db = redisDb;
        KeyName = keyName;
    }

    public Task DeleteAsync() => _db.KeyDeleteAsync(KeyName);

    public Task<byte[]?> DumpAsync() => _db.KeyDumpAsync(KeyName);

    public Task<bool> ExistsAsync() => _db.KeyExistsAsync(KeyName);

    public Task<bool> SetExpirationAsync(TimeSpan? expiry, ExpireWhen when = ExpireWhen.Always)
        => _db.KeyExpireAsync(KeyName, expiry, when);

    public Task<bool> SetExpirationAsync(DateTime? expiry, ExpireWhen when = ExpireWhen.Always)
        => _db.KeyExpireAsync(KeyName, expiry, when);

    public async Task<DateTime?> GetExpirationAsync()
    {
        var universalDt = await _db.KeyExpireTimeAsync(KeyName);
        return universalDt?.ToLocalTime();
    }

    public Task<TimeSpan?> GetIdleTimeAsync() => _db.KeyIdleTimeAsync(KeyName);

    public Task<bool> PersistAsync() => _db.KeyPersistAsync(KeyName);

    public Task<TimeSpan?> GetTtlAsync() => _db.KeyTimeToLiveAsync(KeyName);

    public Task<RedisType> GetRedisTypeAsync() => _db.KeyTypeAsync(KeyName);
}