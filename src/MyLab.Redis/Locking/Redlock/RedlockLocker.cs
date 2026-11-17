using MyLab.Redis.Abstractions;
using MyLab.Redis.Options;
using System.Diagnostics;
using StackExchange.Redis;

namespace MyLab.Redis.Locking.Redlock;

class RedlockLocker(string lockId, IRedisServer[] masters, RedlockOptions opts) : IRedisLocker
{
    readonly KeyNameTool _keyNameTool = new(opts.BaseKey);

    public async Task<RedisAcquisition> TryAcquireOnceAsync(CancellationToken cancellationToken = default)
    {
        var key = _keyNameTool.BuildKey(lockId);
        var redlock = new RedlockLogic(masters, opts);
        var acqRes = await redlock.TryAcquireAsync(key, cancellationToken);

        if (acqRes == null)
            return RedisAcquisition.NotAcquired;

        var lease = new RedlockLease(lockId, redlock, acqRes);
        return RedisAcquisition.CreateAcquired(lease);
    }

    public Task<RedisAcquisition> TryAcquireAsync(CancellationToken cancellationToken = default)
    {
        var key = _keyNameTool.BuildKey(lockId);
        var lockingTime = TimeSpan.FromSeconds(opts.LockingTimeoutSec);
        return TryAcquireCoreAsync(key, lockingTime, cancellationToken);
    }

    public Task<RedisAcquisition> TryAcquireAsync(
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        var key = _keyNameTool.BuildKey(lockId);
        return TryAcquireCoreAsync(key, timeout, cancellationToken);
    }

    async Task<RedisAcquisition> TryAcquireCoreAsync(
        RedisKey key,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        EnsureTimeout(timeout);

        var retryPeriod = TimeSpan.FromSeconds(opts.RetryPeriodSec);

        var redlock = new RedlockLogic(masters, opts);

        var started = Stopwatch.GetTimestamp();
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var acqRes = await redlock.TryAcquireAsync(key, cancellationToken);
            if (acqRes != null)
            {
                var lease = new RedlockLease(lockId, redlock, acqRes);
                return RedisAcquisition.CreateAcquired(lease);
            }

            var remaining = timeout - Stopwatch.GetElapsedTime(started);
            if (remaining <= TimeSpan.Zero)
                return RedisAcquisition.NotAcquired;

            var delay = retryPeriod < remaining ? retryPeriod : remaining;
            if (delay <= TimeSpan.Zero)
                return RedisAcquisition.NotAcquired;

            await Task.Delay(delay, cancellationToken);
        }
    }

    static void EnsureTimeout(TimeSpan timeout)
    {
        if (timeout < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(timeout), "Redlock timeout cannot be negative.");
    }
}
