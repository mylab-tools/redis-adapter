using System.Text.Json;
using MyLab.Redis.Caching;
using MyLab.Redis.Options;
using Xunit.Abstractions;

namespace IntegrationTests.Caching
{
    public class RedisCacheBehavior(ITestOutputHelper output)
    {
        [Fact]
        public async Task ShouldStoreSerializedItem()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var itemId = redis.NewKeyName();
            var item = new SampleItem("alpha", 3);
            IRedisCache cache = redis.Service.Cache("orders");
            var entry = cache.Item(itemId);

            // Act
            var saved = await entry.SetAsync(item);

            // Assert
            Assert.True(saved);
            var stored = await redis.Database.StringGetAsync($"cache:{itemId}");
            Assert.Equal(item, JsonSerializer.Deserialize<SampleItem>(stored.ToString()));
        }

        [Fact]
        public async Task ShouldReturnDefaultWhenItemIsMissing()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            IRedisCache cache = redis.Service.Cache("orders");
            var entry = cache.Item(redis.NewKeyName());

            // Act
            var result = await entry.GetAsync<string>();

            // Assert
            Assert.False(result.Hit);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task ShouldReturnDeserializedItemWhenKeyExists()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var itemId = redis.NewKeyName();
            var item = new SampleItem("beta", 8);
            IRedisCache cache = redis.Service.Cache("orders");
            var entry = cache.Item(itemId);
            if (!await entry.SetAsync(item))
                throw new InvalidOperationException("Item was not stored.");

            // Act
            var result = await entry.GetAsync<SampleItem>();

            // Assert
            Assert.True(result.Hit);
            Assert.Equal(item, result.Value);
        }

        [Fact]
        public async Task ShouldStoreItemFromProviderWhenKeyIsMissing()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var itemId = redis.NewKeyName();
            IRedisCache cache = redis.Service.Cache("orders");
            var entry = cache.Item(itemId);
            var created = new SampleItem("gamma", 1);

            // Act
            var result = await entry.GetAsync<SampleItem>(_ => Task.FromResult(created));

            // Assert
            Assert.False(result.Hit);
            Assert.Equal(created, result.Value);
            var stored = await redis.Database.StringGetAsync($"cache:{itemId}");
            Assert.Equal(created, JsonSerializer.Deserialize<SampleItem>(stored.ToString()));
        }

        [Fact]
        public async Task ShouldReturnStoredItemWithoutCallingProvider()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var itemId = redis.NewKeyName();
            var stored = new SampleItem("delta", 4);
            IRedisCache cache = redis.Service.Cache("orders");
            var entry = cache.Item(itemId);
            if (!await entry.SetAsync(stored))
                throw new InvalidOperationException("Item was not stored.");
            var calls = 0;

            // Act
            var result = await entry.GetAsync<SampleItem>(_ =>
            {
                calls++;
                return Task.FromResult(new SampleItem("other", 0));
            });

            // Assert
            Assert.Equal(0, calls);
            Assert.True(result.Hit);
            Assert.Equal(stored, result.Value);
        }

        [Fact]
        public async Task ShouldRemoveItemWhenInvalidated()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var itemId = redis.NewKeyName();
            IRedisCache cache = redis.Service.Cache("orders");
            var entry = cache.Item(itemId);
            if (!await entry.SetAsync(new SampleItem("epsilon", 2)))
                throw new InvalidOperationException("Item was not stored.");

            // Act
            var removed = await entry.InvalidateAsync();

            // Assert
            Assert.True(removed);
            var stored = await redis.Database.StringGetAsync($"cache:{itemId}");
            Assert.True(stored.IsNull);
        }

        [Fact]
        public async Task ShouldReturnRemainingLifetime()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output, opt =>
            {
                TestTools.ConfigureRedis(opt);
                opt.Cache = new ConfigCacheOptions { ExpirySec = 30 };
            });
            IRedisCache cache = redis.Service.Cache("orders");
            var entry = cache.Item(redis.NewKeyName());
            if (!await entry.SetAsync(new SampleItem("zeta", 5)))
                throw new InvalidOperationException("Item was not stored.");

            // Act
            var ttl = await entry.TtlRemainingAsync();

            // Assert
            Assert.InRange(ttl, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30));
        }

        [Fact]
        public async Task ShouldReturnZeroLifetimeWhenItemIsMissing()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            IRedisCache cache = redis.Service.Cache("orders");
            var entry = cache.Item(redis.NewKeyName());

            // Act
            var ttl = await entry.TtlRemainingAsync();

            // Assert
            Assert.Equal(TimeSpan.Zero, ttl);
        }

        [Fact]
        public async Task ShouldCountKeysThatStartWithBaseKey()
        {
            // Arrange
            var baseKey = "c" + Guid.NewGuid().ToString("N");
            await using var redis = await TestTools.OpenRedisAsync(output, opt =>
            {
                TestTools.ConfigureRedis(opt);
                opt.Cache = new ConfigCacheOptions { BaseKey = baseKey };
            });
            IRedisCache cache = redis.Service.Cache("orders");
            if (!await cache.Item(redis.NewKeyName()).SetAsync(new SampleItem("one", 1))
                || !await cache.Item(redis.NewKeyName()).SetAsync(new SampleItem("two", 2)))
                throw new InvalidOperationException("Item was not stored.");
            await redis.Database.StringSetAsync("other-" + redis.NewKeyName(), "outside");

            // Act
            var count = await cache.GetItemsCountAsync();

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task ShouldSkipSaveWhenItemIsLocked()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var itemId = redis.NewKeyName();
            await using var held = await redis.Service.Lock($"cache:{itemId}").TryAcquireOnceAsync()
                ?? throw new InvalidOperationException("Lock was not acquired.");
            IRedisCache cache = redis.Service.Cache("orders");

            // Act
            var saved = await cache.Item(itemId).SetAsync(new SampleItem("locked", 9));

            // Assert
            Assert.False(saved);
            var stored = await redis.Database.StringGetAsync($"cache:{itemId}");
            Assert.True(stored.IsNull);
        }

        [Fact]
        public async Task ShouldNotCallProviderWhenItemIsLocked()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var itemId = redis.NewKeyName();
            await using var held = await redis.Service.Lock($"cache:{itemId}").TryAcquireOnceAsync()
                                   ?? throw new InvalidOperationException("Lock was not acquired.");
            IRedisCache cache = redis.Service.Cache("orders");

            bool providerCalled = false;
            
            // Act
            var cacheRes = await cache.Item(itemId).GetAsync(_ =>
            {
                providerCalled = true;
                return Task.FromResult(string.Empty);
            });

            var stored = await redis.Database.StringGetAsync($"cache:{itemId}");

            // Assert
            Assert.False(providerCalled);
            Assert.False(cacheRes.Hit);
            Assert.Null(cacheRes.Value);
            Assert.True(stored.IsNull);
        }
    }

    record SampleItem(string Name, int Count);
}
