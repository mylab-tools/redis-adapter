using MyLab.Redis.Abstractions;
using StackExchange.Redis;

namespace MyLab.Redis.Locking;

/// <summary>
/// A lock held on a majority of Redis masters.
/// </summary>
public interface IRedisLockLease : IAsyncDisposable
{
    /// <summary>
    /// Identifier passed to <see cref="IRedisService.Lock"/>.
    /// </summary>
    string LockId { get; }

    /// <summary>
    /// Redis key that stores the lock token.
    /// </summary>
    RedisKey Key { get; }

    /// <summary>
    /// Time left after the last successful acquire or prolong, minus elapsed time and clock drift.
    /// </summary>
    TimeSpan Validity { get; } 

    /// <summary>
    /// UTC moment until which <see cref="Validity"/> was considered positive.
    /// </summary>
    DateTime ValidUntil { get; }

    /// <summary>
    /// Reads the lock from a majority of masters and checks that this lease still owns it.
    /// </summary>
    Task<bool> IsAcquiredAsync();

    /// <summary>
    /// Prolongs the lock for expiry from options.
    /// </summary>
    /// <returns><see langword="true"/> when a majority accepted the new expiry and the remaining validity is positive.</returns>
    Task<bool> ProlongAsync();
}