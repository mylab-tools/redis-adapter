using MyLab.Redis.Keys;
using StackExchange.Redis;
using Xunit.Abstractions;

namespace IntegrationTests.Keys
{
    public class HashRedisKeyBehavior(ITestOutputHelper output)
    {
        [Fact]
        public async Task ShouldDecreaseFieldInteger()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("count", "10");

            // Act
            var value = await key.DecrementAsync("count", 3);

            // Assert
            Assert.Equal(7, value);
        }

        [Fact]
        public async Task ShouldDecreaseFieldFloatingValue()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("count", "10.5");

            // Act
            var value = await key.DecrementAsync("count", 0.5);

            // Assert
            Assert.Equal(10, value);
        }

        [Fact]
        public async Task ShouldRemoveField()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("a", "1");

            // Act
            var removed = await key.DeleteFieldAsync("a");

            // Assert
            Assert.True(removed);
        }

        [Fact]
        public async Task ShouldRemoveSeveralFields()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync([new HashEntry("a", "1"), new HashEntry("b", "2")]);

            // Act
            var removed = await key.DeleteFieldsAsync(["a", "b"]);

            // Assert
            Assert.Equal(2, removed);
        }

        [Fact]
        public async Task ShouldReportFieldPresence()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("a", "1");

            // Act
            var exists = await key.FieldExistsAsync("a");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ShouldReturnFieldValue()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("a", "1");

            // Act
            var value = await key.GetAsync("a");

            // Assert
            Assert.Equal("1", value.ToString());
        }

        [Fact]
        public async Task ShouldReturnSeveralFieldValues()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync([new HashEntry("a", "1"), new HashEntry("b", "2")]);

            // Act
            var values = await key.GetAsync(["b", "a"]);

            // Assert
            Assert.Equal(["2", "1"], values.Select(value => value.ToString()).ToArray());
        }

        [Fact]
        public async Task ShouldReturnAllFields()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync([new HashEntry("a", "1"), new HashEntry("b", "2")]);

            // Act
            var entries = await key.GetAllAsync();

            // Assert
            Assert.Equal(2, entries.Length);
            Assert.Contains(entries, entry => entry.Name == "a" && entry.Value == "1");
            Assert.Contains(entries, entry => entry.Name == "b" && entry.Value == "2");
        }

        [Fact]
        public async Task ShouldIncreaseFieldInteger()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("count", "10");

            // Act
            var value = await key.IncrementAsync("count", 4);

            // Assert
            Assert.Equal(14, value);
        }

        [Fact]
        public async Task ShouldIncreaseFieldFloatingValue()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("count", "10.5");

            // Act
            var value = await key.IncrementAsync("count", 0.5);

            // Assert
            Assert.Equal(11, value);
        }

        [Fact]
        public async Task ShouldReturnFieldNames()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync([new HashEntry("a", "1"), new HashEntry("b", "2")]);

            // Act
            var names = await key.KeysAsync();

            // Assert
            Assert.Equal(["a", "b"], names.Select(name => name.ToString()).Order().ToArray());
        }

        [Fact]
        public async Task ShouldReturnFieldCount()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync([new HashEntry("a", "1"), new HashEntry("b", "2")]);

            // Act
            var length = await key.LengthAsync();

            // Assert
            Assert.Equal(2, length);
        }

        [Fact]
        public async Task ShouldEnumerateMatchingFields()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync([new HashEntry("a1", "1"), new HashEntry("b1", "2")]);
            var names = new List<string>();

            // Act
            await foreach (var entry in key.ScanAsync("a*"))
                names.Add(entry.Name.ToString());

            // Assert
            Assert.Equal(["a1"], names);
        }

        [Fact]
        public async Task ShouldStoreSeveralFields()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);

            // Act
            await key.SetAsync([new HashEntry("a", "1"), new HashEntry("b", "2")]);
            var length = await key.LengthAsync();

            // Assert
            Assert.Equal(2, length);
        }

        [Fact]
        public async Task ShouldStoreField()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);

            // Act
            var created = await key.SetAsync("a", "1");

            // Assert
            Assert.True(created);
        }

        [Fact]
        public async Task ShouldReturnFieldValueLength()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("a", "hello");

            // Act
            var length = await key.StringLengthAsync("a");

            // Assert
            Assert.Equal(5, length);
        }

        [Fact]
        public async Task ShouldReturnFieldValues()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new HashRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync([new HashEntry("a", "1"), new HashEntry("b", "2")]);

            // Act
            var values = await key.ValuesAsync();

            // Assert
            Assert.Equal(["1", "2"], values.Select(value => value.ToString()).Order().ToArray());
        }
    }
}
