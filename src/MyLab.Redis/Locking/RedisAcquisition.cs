namespace MyLab.Redis.Locking;

public record RedisAcquisition(bool Acquired, IRedisLockLease? Lease) : IAsyncDisposable
{
    public static readonly RedisAcquisition NotAcquired = new RedisAcquisition(false, null);

    public static RedisAcquisition CreateAcquired(IRedisLockLease lease)
    {
        if (lease == null) throw new ArgumentNullException(nameof(lease));

        return new RedisAcquisition(true, lease);
    }

    public ValueTask DisposeAsync()
    {
        return Lease?.DisposeAsync() ?? ValueTask.CompletedTask;
    }
}