using StackExchange.Redis;

namespace MyLab.Redis.Locking;

sealed class RedlockLease(string lockId, Redlock redlock, AcquireResult acquireRes) : IRedisLockLease
{
    private int _disposed;

    public string LockId => lockId;

    public RedisKey Key => acquireRes.Key;

    public TimeSpan Validity
    {
        get;
        private set
        {
            field = value;
            ValidUntil = DateTime.UtcNow + Validity;
        }
    } = acquireRes.Validity;

    public DateTime ValidUntil { get; private set; }

    public Task<bool> IsAcquiredAsync()
    {
        ObjectDisposedException.ThrowIf(_disposed != 0, this);
        return redlock.IsHeldAsync(acquireRes.Key, acquireRes.Token);
    }

    public async Task<bool> ProlongAsync()
    {
        ObjectDisposedException.ThrowIf(_disposed != 0, this);

        var validity = await redlock.ProlongAsync(acquireRes.Key, acquireRes.Token);
        if (validity == null)
            return false;

        Validity = validity.Value;
        return true;
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
            return;

        await redlock.ReleaseAsync(acquireRes.Key, acquireRes.Token);
    }
}