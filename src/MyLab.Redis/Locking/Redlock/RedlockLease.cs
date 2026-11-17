using StackExchange.Redis;

namespace MyLab.Redis.Locking.Redlock;

sealed class RedlockLease(string lockId, RedlockLogic redlock, RedlockAcquireResult redlockAcquireRes) : IRedisLockLease
{
    private int _disposed;

    public string LockId => lockId;

    public RedisKey Key => redlockAcquireRes.Key;

    public TimeSpan Validity
    {
        get;
        private set
        {
            field = value;
            ValidUntil = DateTime.UtcNow + Validity;
        }
    } = redlockAcquireRes.Validity;

    public DateTime ValidUntil { get; private set; }

    public Task<bool> IsAcquiredAsync()
    {
        ObjectDisposedException.ThrowIf(_disposed != 0, this);
        return redlock.IsHeldAsync(redlockAcquireRes.Key, redlockAcquireRes.Token);
    }

    public async Task<bool> ProlongAsync()
    {
        ObjectDisposedException.ThrowIf(_disposed != 0, this);

        var validity = await redlock.ProlongAsync(redlockAcquireRes.Key, redlockAcquireRes.Token);
        if (validity == null)
            return false;

        Validity = validity.Value;
        return true;
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
            return;

        await redlock.ReleaseAsync(redlockAcquireRes.Key, redlockAcquireRes.Token);
    }
}