using System;
using System.Threading.Tasks;
using MyLab.Redis.ObjectModel;
using Xunit;

namespace IntegrationTests
{
    public class RedisCacheBehavior
    {
        [Fact]
        public async Task ShouldAddItemIfNotExists()
        {
            await TestTools.PerformTest(async (redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(TestTools.CacheDefaultName);
                //Act
                var cacheItem = await cache.FetchAsync("foo", () => new CacheItem
                {
                    Id = 1,
                    Value = 2
                });

                //Assert
                Assert.NotNull(cacheItem);
                Assert.Equal(1, cacheItem.Id);
                Assert.Equal(2, cacheItem.Value);

            });
        }

        [Fact]
        public async Task ShouldProvideItemFromRedisInsteadCreateNewOne()
        {
            await TestTools.PerformTest(async (redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(TestTools.CacheDefaultName);
                CacheItem cacheItem = null;

                //Act
                for (int i = 0; i < 2; i++)
                {
                    cacheItem = await cache.FetchAsync("foo", () => new CacheItem
                    {
                        Id = i,
                        Value = i+1
                    });
                }

                //Assert
                Assert.NotNull(cacheItem);
                Assert.Equal(0, cacheItem.Id);
                Assert.Equal(1, cacheItem.Value);

            });
        }

        [Fact]
        public async Task ShouldSetDefaultExpiration()
        {
            await TestTools.PerformTest(async (redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(TestTools.Cache100MsName);

                //Act
                await cache.FetchAsync("foo", () => new CacheItem{ Id = 1 });

                await Task.Delay(150);
                
                var cacheItem = await cache.FetchAsync("foo", () => new CacheItem{ Id = 2 });

                //Assert
                Assert.Equal(2, cacheItem.Id);

            });
        }

        [Fact]
        public async Task ShouldSetExpiration()
        {
            await TestTools.PerformTest(async (redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(TestTools.Cache1000MsName);

                //Act
                await cache.FetchAsync("foo", () => new CacheItem { Id = 1 }, TimeSpan.FromMilliseconds(100));

                await Task.Delay(150);

                var cacheItem = await cache.FetchAsync("foo", () => new CacheItem { Id = 2 });

                //Assert
                Assert.Equal(2, cacheItem.Id);

            });
        }

        [Fact]
        public async Task ShouldCountItems()
        {
            await TestTools.PerformTest(async (redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(TestTools.Cache1MinName);

                for (int i = 0; i < 260; i++)
                {
                    await cache.FetchAsync("foo-" + i, () => new CacheItem { Id = i });
                }

                //Act

                var count = await cache.Count();

                //Assert
                Assert.Equal(260, count);
                
            });
        }

        class CacheItem
        {
            public int Id { get; set; }
            public int Value { get; set; }
        }
    }
}
