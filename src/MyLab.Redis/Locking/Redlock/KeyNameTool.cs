using StackExchange.Redis;

namespace MyLab.Redis.Locking.Redlock;

class KeyNameTool(string baseKey)
{
    public static void EnsureResource(string lockId)
    {
        if (string.IsNullOrWhiteSpace(lockId))
            throw new ArgumentException("Lock lockId is required.", nameof(lockId));
    }

    public RedisKey BuildKey(string lockId)
    {
        EnsureResource(lockId);

        return $"{baseKey}:{lockId}";
    }
}