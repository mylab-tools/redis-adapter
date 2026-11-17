using MyLab.Redis.Locking;
using Xunit.Abstractions;

namespace IntegrationTests.Locking
{
    public class RedlockBehavior(ITestOutputHelper output)
    {
        [Fact]
        public async Task ShouldReturnLeaseWhenResourceIsFree()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output, TestTools.ConfigureThreeRedis);
            var lockId = redis.NewKeyName();
            IRedisLocker locker = redis.Service.Lock(lockId);

            // Act
            await using var lease = await locker.TryAcquireOnceAsync();

            // Assert
            Assert.NotNull(lease);
            Assert.Equal(lockId, lease.LockId);
            Assert.Equal($"redlock:{lockId}", lease.Key.ToString());
            Assert.True(lease.Validity > TimeSpan.Zero);
            Assert.True(await lease.IsAcquiredAsync());
        }

        [Fact]
        public async Task ShouldAcquireResourceBeforeTimeoutElapses()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output, TestTools.ConfigureThreeRedis);
            var lockId = redis.NewKeyName();
            IRedisLocker locker = redis.Service.Lock(lockId);

            // Act
            await using var lease = await locker.TryAcquireAsync();

            // Assert
            Assert.NotNull(lease);
            Assert.True(await lease.IsAcquiredAsync());
        }

        [Fact]
        public async Task ShouldDenyAcquireWhenResourceIsAlreadyHeld()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output, TestTools.ConfigureThreeRedis);
            var lockId = redis.NewKeyName();
            IRedisLocker holder = redis.Service.Lock(lockId);
            await using var held = await holder.TryAcquireOnceAsync()
                ?? throw new InvalidOperationException("Lock was not acquired.");
            IRedisLocker contender = redis.Service.Lock(lockId);

            // Act
            await using var second = await contender.TryAcquireOnceAsync();

            // Assert
            Assert.Null(second);
            Assert.True(await held.IsAcquiredAsync());
        }

        [Fact]
        public async Task ShouldStopWaitingWhenTimeoutElapses()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output, TestTools.ConfigureThreeRedis);
            var lockId = redis.NewKeyName();
            IRedisLocker holder = redis.Service.Lock(lockId);
            await using var held = await holder.TryAcquireOnceAsync()
                ?? throw new InvalidOperationException("Lock was not acquired.");
            IRedisLocker contender = redis.Service.Lock(lockId);

            // Act
            await using var second = await contender.TryAcquireAsync(TimeSpan.FromMilliseconds(200));

            // Assert
            Assert.Null(second);
        }

        [Fact]
        public async Task ShouldReleaseResourceWhenLeaseIsDisposed()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output, TestTools.ConfigureThreeRedis);
            var lockId = redis.NewKeyName();
            IRedisLocker locker = redis.Service.Lock(lockId);
            var lease = await locker.TryAcquireOnceAsync()
                ?? throw new InvalidOperationException("Lock was not acquired.");

            // Act
            await lease.DisposeAsync();

            // Assert
            await using var again = await locker.TryAcquireOnceAsync();
            Assert.NotNull(again);
        }

        [Fact]
        public async Task ShouldExtendHeldResourceWhenProlonged()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output, TestTools.ConfigureThreeRedis);
            var lockId = redis.NewKeyName();
            IRedisLocker locker = redis.Service.Lock(lockId);
            await using var lease = await locker.TryAcquireOnceAsync()
                ?? throw new InvalidOperationException("Lock was not acquired.");

            // Act
            var prolonged = await lease.ProlongAsync();

            // Assert
            Assert.True(prolonged);
            Assert.True(lease.Validity > TimeSpan.Zero);
            Assert.True(await lease.IsAcquiredAsync());
        }

        [Fact]
        public async Task ShouldRejectEmptyLockId()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output, TestTools.ConfigureThreeRedis);
            IRedisLocker locker = redis.Service.Lock(" ");

            // Act
            var acquire = () => locker.TryAcquireOnceAsync();

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(acquire);
        }

        [Fact]
        public async Task ShouldRejectNegativeTimeout()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output, TestTools.ConfigureThreeRedis);
            IRedisLocker locker = redis.Service.Lock(redis.NewKeyName());

            // Act
            var acquire = () => locker.TryAcquireAsync(TimeSpan.FromSeconds(-1));

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(acquire);
        }
    }
}
