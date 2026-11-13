using MyLab.Redis.Keys;
using StackExchange.Redis;
using Xunit.Abstractions;

namespace IntegrationTests.Keys
{
    public class StringRedisKeyBehavior(ITestOutputHelper output)
    {
        [Fact]
        public async Task ShouldRemoveKey()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("value");

            // Act
            await key.DeleteAsync();
            var exists = await key.ExistsAsync();

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task ShouldReturnSerializedValue()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("value");

            // Act
            var dump = await key.DumpAsync();

            // Assert
            Assert.NotNull(dump);
            Assert.NotEmpty(dump);
        }

        [Fact]
        public async Task ShouldReportExistingKey()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("value");

            // Act
            var exists = await key.ExistsAsync();

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ShouldSetRelativeExpiration()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("value");

            // Act
            var set = await key.SetExpirationAsync(TimeSpan.FromMinutes(5));

            // Assert
            Assert.True(set);
        }

        [Fact]
        public async Task ShouldSetAbsoluteExpiration()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("value");

            // Act
            var set = await key.SetExpirationAsync(DateTime.UtcNow.AddMinutes(30));

            // Assert
            Assert.True(set);
        }

        [Fact]
        public async Task ShouldReturnExpirationMoment()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("value");
            var expiry = DateTime.UtcNow.AddMinutes(30);
            await key.SetExpirationAsync(expiry);

            // Act
            var actual = await key.GetExpirationAsync();

            // Assert
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expiry.ToLocalTime().AddSeconds(-2), expiry.ToLocalTime().AddSeconds(2));
        }

        [Fact]
        public async Task ShouldReturnIdleTime()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("value");

            // Act
            var idle = await key.GetIdleTimeAsync();

            // Assert
            Assert.NotNull(idle);
        }

        [Fact]
        public async Task ShouldClearExpiration()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("value");
            await key.SetExpirationAsync(TimeSpan.FromMinutes(5));

            // Act
            var cleared = await key.PersistAsync();
            var ttl = await key.GetTtlAsync();

            // Assert
            Assert.True(cleared);
            Assert.Null(ttl);
        }

        [Fact]
        public async Task ShouldReturnRemainingLifetime()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("value");
            await key.SetExpirationAsync(TimeSpan.FromMinutes(5));

            // Act
            var ttl = await key.GetTtlAsync();

            // Assert
            Assert.NotNull(ttl);
            Assert.InRange(ttl.Value.TotalSeconds, 1, 300);
        }

        [Fact]
        public async Task ShouldReportStringType()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("value");

            // Act
            var type = await key.GetRedisTypeAsync();

            // Assert
            Assert.Equal(RedisType.String, type);
        }

        [Fact]
        public async Task ShouldDecreaseInteger()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("10");

            // Act
            var value = await key.DecrementAsync(3);

            // Assert
            Assert.Equal(7, value);
        }

        [Fact]
        public async Task ShouldDecreaseFloatingValue()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("10.5");

            // Act
            var value = await key.DecrementAsync(0.5);

            // Assert
            Assert.Equal(10, value);
        }

        [Fact]
        public async Task ShouldReturnStoredString()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("hello");

            // Act
            var value = await key.GetAsync();

            // Assert
            Assert.Equal("hello", value.ToString());
        }

        [Fact]
        public async Task ShouldReturnSubstring()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("hello");

            // Act
            var value = await key.GetRangeAsync(1, 3);

            // Assert
            Assert.Equal("ell", value.ToString());
        }

        [Fact]
        public async Task ShouldReplaceAndReturnPreviousValue()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("old");

            // Act
            var previous = await key.GetSetAsync("new");

            // Assert
            Assert.Equal("old", previous.ToString());
        }

        [Fact]
        public async Task ShouldIncreaseInteger()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("10");

            // Act
            var value = await key.IncrementAsync(4);

            // Assert
            Assert.Equal(14, value);
        }

        [Fact]
        public async Task ShouldIncreaseFloatingValue()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("10.5");

            // Act
            var value = await key.IncrementAsync(0.5);

            // Assert
            Assert.Equal(11, value);
        }

        [Fact]
        public async Task ShouldReturnStringLength()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("hello");

            // Act
            var length = await key.LengthAsync();

            // Assert
            Assert.Equal(5, length);
        }

        [Fact]
        public async Task ShouldStoreString()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);

            // Act
            var stored = await key.SetAsync("bar");

            // Assert
            Assert.True(stored);
        }

        [Fact]
        public async Task ShouldOverwriteStringFromOffset()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("hello");

            // Act
            var length = await key.SetRangeAsync(1, "i");

            // Assert
            Assert.Equal(5, (long)length);
        }
    }
}
