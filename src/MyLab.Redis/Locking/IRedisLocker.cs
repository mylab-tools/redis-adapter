namespace MyLab.Redis.Locking;

/// <summary>
/// Acquires a distributed lock on a majority of Redis masters.
/// </summary>
public interface IRedisLocker
{
    /// <summary>
    /// Tries to acquire lock once.
    /// </summary>
    /// <returns>The acquired lock</returns>
    public Task<RedisAcquisition> TryAcquireOnceAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Tries to acquire lock until LockingTimeout elapses.
    /// </summary>
    /// <returns>The acquired lock</returns>
    public Task<RedisAcquisition> TryAcquireAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Tries to acquire lock until <paramref name="timeout"/> elapses.
    /// </summary>
    /// <returns>The acquired lock</returns>
    public Task<RedisAcquisition> TryAcquireAsync(
        TimeSpan timeout,
        CancellationToken cancellationToken = default);
}