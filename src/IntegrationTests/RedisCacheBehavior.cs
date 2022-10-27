using System;
using System.Threading.Tasks;
using MyLab.Redis;
using MyLab.Redis.ObjectModel;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class RedisCacheBehavior
    {
        const string Cache100MsName = "100msCache";
        const string Cache1000MsName = "1000msCache";
        const string Cache1MinName = "1minCache";
        const string CacheDefaultName = Cache100MsName;

        private readonly Action<RedisOptions> _editOptions;

        private readonly ITestOutputHelper _output;

        public RedisCacheBehavior(ITestOutputHelper output)
        {
            _output = output;

            _editOptions = o =>
            {
                o.Caching = new CachingOptions
                {
                    KeyPrefix = "cache",
                    Caches = new[]
                    {
                        new CacheOptions
                        {
                            Name = Cache100MsName,
                            DefaultExpiry = TimeSpan.FromMilliseconds(100).ToString()
                        },
                        new CacheOptions
                        {
                            Name = Cache1000MsName,
                            DefaultExpiry = TimeSpan.FromMilliseconds(1000).ToString()
                        },
                        new CacheOptions
                        {
                            Name = Cache1MinName,
                            DefaultExpiry = TimeSpan.FromMinutes(1).ToString()
                        }
                    }
                };
            };
        }

        [Fact]
        public async Task ShouldAddItem()
        {
            await TestTools.PerformTest(_output, async(redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(CacheDefaultName);
                var cacheItem = new CacheItem
                {
                    Id = 0
                };

                //Act
                await cache.AddAsync("foo", cacheItem);

                var cacheItemRes = await cache.FetchAsync("foo", () => new CacheItem
                {
                    Id = 1
                });

                //Assert
                Assert.NotNull(cacheItemRes);
                Assert.Equal(0, cacheItemRes.Id);

            }, _editOptions);
        }

        [Fact]
        public async Task ShouldAddItemIfNotExists()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(CacheDefaultName);
                //Act
                var cacheItem = await cache.FetchAsync("foo", () => new CacheItem
                {
                    Id = 1
                });

                //Assert
                Assert.NotNull(cacheItem);
                Assert.Equal(1, cacheItem.Id);

            }, _editOptions);
        }

        [Fact]
        public async Task ShouldProvideDefaultWhenTryFetchAndItemNotExists()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(CacheDefaultName);
                //Act
                var cacheItem = await cache.TryFetchAsync<CacheItem>("foo");

                //Assert
                Assert.Null(cacheItem);
            }, _editOptions);
        }

        [Fact]
        public async Task ShouldProvideItemWhenTryFetchAndItemExists()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(CacheDefaultName);
                await cache.AddAsync("foo", new CacheItem
                {
                    Id = 5
                });

                //Act
                var cacheItem = await cache.TryFetchAsync<CacheItem>("foo");

                //Assert
                Assert.NotNull(cacheItem);
                Assert.Equal(5, cacheItem.Id);
            }, _editOptions);
        }

        [Fact]
        public async Task ShouldProvideItemFromRedisInsteadCreateNewOne()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(CacheDefaultName);
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

            }, _editOptions);
        }

        [Fact]
        public async Task ShouldSetDefaultExpiration()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(Cache100MsName);

                //Act
                await cache.FetchAsync("foo", () => new CacheItem{ Id = 1 });

                await Task.Delay(150);
                
                var cacheItem = await cache.FetchAsync("foo", () => new CacheItem{ Id = 2 });

                //Assert
                Assert.Equal(2, cacheItem.Id);

            }, _editOptions);
        }

        [Fact]
        public async Task ShouldSetExpiration()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(Cache1000MsName);

                //Act
                await cache.FetchAsync("foo", () => new CacheItem { Id = 1 }, TimeSpan.FromMilliseconds(100));

                await Task.Delay(150);

                var cacheItem = await cache.FetchAsync("foo", () => new CacheItem { Id = 2 });

                //Assert
                Assert.Equal(2, cacheItem.Id);

            }, _editOptions);
        }

        [Fact]
        public async Task ShouldCountItems()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var cache = redis.Db().Cache(Cache1MinName);

                for (int i = 0; i < 260; i++)
                {
                    await cache.FetchAsync(testKey + "-" + i, () => new CacheItem { Id = i });
                }

                //Act

                var count = await cache.CountAsync();

                //Assert
                Assert.Equal(260, count);
                
            }, _editOptions);
        }

        class CacheItem
        {
            public int Id { get; set; }
            public int Value { get; set; }
        }
    }
}
