namespace MyLab.Redis.Locking;

public interface IRedisLocker
{
    /// <summary>
    /// Tries to acquire lock once.
    /// </summary>
    /// <returns>The acquired lock, or <see langword="null"/> when the majority was not reached.</returns>
    public Task<IRedisLockLease?> TryAcquireOnceAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Tries to acquire lock until LockingTimeout elapses.
    /// </summary>
    /// <returns>The acquired lock, or <see langword="null"/> when the timeout elapses.</returns>
    public Task<IRedisLockLease?> TryAcquireAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Tries to acquire lock until <paramref name="timeout"/> elapses.
    /// </summary>
    /// <returns>The acquired lock, or <see langword="null"/> when the timeout elapses.</returns>
    public Task<IRedisLockLease?> TryAcquireAsync(
        TimeSpan timeout,
        CancellationToken cancellationToken = default);
}