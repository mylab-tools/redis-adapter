using StackExchange.Redis;

namespace MyLab.Redis.Locking;

public interface IRedisLockLease : IAsyncDisposable
{
    string LockId { get; }

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