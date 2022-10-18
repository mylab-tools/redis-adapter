using System;
using System.Threading.Tasks;
using MyLab.Redis;
using MyLab.Redis.ObjectModel;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class RedlockBehavior
    {
        private readonly ITestOutputHelper _output;

        private readonly Action<RedisOptions> _editOptions;

        public RedlockBehavior(ITestOutputHelper output)
        {
            _output = output;

            _editOptions = o =>
            {
                o.Locking = new LockingOptions
                {
                    Locks = new[]
                    {
                        new LockOptions
                        {
                            Name = "foo",
                            Expiry = TimeSpan.FromMilliseconds(500).ToString()
                        }
                    }
                };
            };
        }

        [Fact]
        public async Task ShouldGetAcquiredIfLocked()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var locker = redis.Db().CreateLocker("foo");
                await using var lockAttempt = await locker.TryLockOnceAsync();

                //Act
                var acquired = await lockAttempt.Lock.IsAcquiredAsync();

                //Assert
                Assert.True(acquired);
            }, _editOptions);
        }

        [Fact]
        public async Task ShouldNotGetAcquiredIfNotLocked()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var locker = redis.Db().CreateLocker("foo");

                await using var lockAttempt = await locker.TryLockOnceAsync();
                await Task.Delay(600);

                //Act
                var acquired = await lockAttempt.Lock.IsAcquiredAsync();

                //Assert
                Assert.False(acquired);
            }, _editOptions);
        }

        [Fact]
        public async Task ShouldProlong()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var locker = redis.Db().CreateLocker("foo");

                await using var lockAttempt = await locker.TryLockOnceAsync();
               
                //Act
                await lockAttempt.Lock.ProlongAsync(TimeSpan.FromSeconds(5));
                await Task.Delay(600);
                var acquired = await lockAttempt.Lock.IsAcquiredAsync();

                //Assert
                Assert.True(acquired);
            }, _editOptions);
        }

        [Fact]
        public async Task ShouldUnlockWhenDispose()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var locker = redis.Db().CreateLocker("foo");

                var lockAttempt = await locker.TryLockOnceAsync();

                //Act
                await lockAttempt.Lock.DisposeAsync();

                var acquired = await locker.IsAcquiredAsync();

                //Assert
                Assert.False(acquired);
            }, _editOptions);
        }
    }
}