using MyLab.Redis.Keys;
using Xunit.Abstractions;

namespace IntegrationTests.Keys
{
    public class StringRedisKeyBehavior(ITestOutputHelper output)
    {
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
