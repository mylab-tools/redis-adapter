using MyLab.Redis.Abstractions;
using MyLab.Redis.Options;
using System.Diagnostics;
using StackExchange.Redis;

namespace MyLab.Redis.Locking;

class RedlockLocker(string lockId, IRedisServer[] masters, RedlockOptions opts) : IRedisLocker
{
    readonly KeyNameTool _keyNameTool = new(opts.BaseKey);

    public async Task<IRedisLockLease?> TryAcquireOnceAsync(CancellationToken cancellationToken = default)
    {
        var key = _keyNameTool.BuildKey(lockId);
        var redlock = new Redlock(masters, opts);
        var acqRes = await redlock.TryAcquireAsync(key, cancellationToken);

        return acqRes != null
            ? new RedlockLease(lockId, redlock, acqRes)
            : null;
    }

    public Task<IRedisLockLease?> TryAcquireAsync(CancellationToken cancellationToken = default)
    {
        var key = _keyNameTool.BuildKey(lockId);
        var lockingTime = TimeSpan.FromSeconds(opts.LockingTimeoutSec);
        return TryAcquireCoreAsync(key, lockingTime, cancellationToken);
    }

    public Task<IRedisLockLease?> TryAcquireAsync(
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        var key = _keyNameTool.BuildKey(lockId);
        return TryAcquireCoreAsync(key, timeout, cancellationToken);
    }

    async Task<IRedisLockLease?> TryAcquireCoreAsync(
        RedisKey key,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        EnsureTimeout(timeout);

        var retryPeriod = TimeSpan.FromSeconds(opts.RetryPeriodSec);

        var redlock = new Redlock(masters, opts);

        var started = Stopwatch.GetTimestamp();
        while (true)
        {
            var acqRes = await redlock.TryAcquireAsync(key, cancellationToken);
            if (acqRes != null)
                return new RedlockLease(lockId, redlock, acqRes);

            var remaining = timeout - Stopwatch.GetElapsedTime(started);
            if (remaining <= TimeSpan.Zero)
                return null;

            var delay = retryPeriod < remaining ? retryPeriod : remaining;
            if (delay <= TimeSpan.Zero)
                return null;

            await Task.Delay(delay, cancellationToken);
        }
    }

    static void EnsureTimeout(TimeSpan timeout)
    {
        if (timeout < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(timeout), "Redlock timeout cannot be negative.");
    }
}
