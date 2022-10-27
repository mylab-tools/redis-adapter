using System;
using System.Linq;
using System.Threading.Tasks;
using MyLab.Redis;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class RedlockerBehavior
    {
        private readonly ITestOutputHelper _output;

        private readonly Action<RedisOptions> _editOptions;

        public RedlockerBehavior(ITestOutputHelper output)
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
                            RetryPeriod = TimeSpan.FromMilliseconds(500).ToString()
                        }
                    }
                };
            };
        }

        [Fact]
        public async Task ShouldLock()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var locker = redis.Db().CreateLocker("foo");

                //Act
                await using var lockAttempt = await locker.TryLockOnceAsync();

                //Assert
                Assert.True(lockAttempt.Acquired);
            }, _editOptions);
        }

        [Fact]
        public async Task ShouldStopTryLockWhenTimeout()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var locker1 = redis.Db().CreateLocker("foo");
                var locker2 = redis.Db().CreateLocker("foo");

                await using var lockAttempt1 = await locker1.TryLockOnceAsync();

                //Act

                await using var lockAttempt2 = await locker2.TryLockAsync();

                //Assert
                Assert.False(lockAttempt2.Acquired);
            }, _editOptions);
        }

        [Fact]
        public async Task ShouldStopTryLockWhenTimeoutWithSameRetryPeriod()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var locker1 = redis.Db().CreateLocker("foo");
                var locker2 = redis.Db().CreateLocker("foo");

                await using var lockAttempt1 = await locker1.TryLockOnceAsync();

                //Act

                await using var lockAttempt2 = await locker2.TryLockAsync();

                //Assert
                Assert.False(lockAttempt2.Acquired);
            }, o =>
            {
                _editOptions(o);
                var opt = o.Locking.Locks.First(l => l.Name == "foo");
                opt.DefaultTimeout = "00:00:01";
                opt.RetryPeriod = "00:00:01";
            });
        }

        [Fact]
        public async Task ShouldNotLockIfAlreadyLocked()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var locker1 = redis.Db().CreateLocker("foo");
                var locker2 = redis.Db().CreateLocker("foo");

                await using var lockAttempt1 = await locker1.TryLockOnceAsync();

                //Act

                await using var lockAttempt2 = await locker2.TryLockOnceAsync();

                //Assert
                Assert.False(lockAttempt2.Acquired);
            }, _editOptions);
        }

        [Fact]
        public async Task ShouldGetAcquiredIfLocked()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var locker1 = redis.Db().CreateLocker("foo");
                var locker2 = redis.Db().CreateLocker("foo");

                await using var lockAttempt = await locker1.TryLockOnceAsync();

                //Act

                var acquired = await locker2.IsAcquiredAsync();

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

                //Act
                var acquired = await locker.IsAcquiredAsync();

                //Assert
                Assert.False(acquired);
            }, _editOptions);
        }

        [Fact]
        public async Task ShouldWaitForUnlocking()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var locker1 = redis.Db().CreateLocker("foo");
                var locker2 = redis.Db().CreateLocker("foo");
                
                //Act
                await locker1.TryLockOnceAsync(); // <-- will be expired 
                await using var lockAttempt = await locker2.TryLockAsync();
                
                //Assert
                Assert.True(lockAttempt.Acquired);

            }, o =>
            {
                o.Locking = new LockingOptions
                {
                    Locks = new[]
                    {
                        new LockOptions
                        {
                            Name = "foo",
                            Expiry = TimeSpan.FromSeconds(1).ToString()
                        }
                    }
                };
            });
        }
    }
}
