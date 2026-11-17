using MyLab.Redis.Abstractions;
using MyLab.Redis.Keys;
using Xunit.Abstractions;

namespace IntegrationTests.Core
{
    public class RedisServerBehavior(ITestOutputHelper output)
    {
        [Fact]
        public async Task ShouldReturnRoundTripTime()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            IRedisServer server = redis.Service.Server();

            // Act
            var latency = await server.PingAsync();

            // Assert
            Assert.True(latency >= TimeSpan.Zero);
        }

        [Fact]
        public async Task ShouldReturnSentMessage()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            IRedisServer server = redis.Service.Server();

            // Act
            var value = await server.EchoAsync("redis");

            // Assert
            Assert.Equal("redis", value.ToString());
        }

        [Fact]
        public async Task ShouldListStoredKey()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output, TestTools.ConfigureRedisAdmin);
            var keyName = redis.NewKeyName();
            var key = new StringRedisKey(keyName, redis.Database);
            await key.SetAsync("value");
            IRedisServer server = redis.Service.Server();
            var names = new List<string>();

            // Act
            await foreach (var found in server.GetKeysAsync(keyName))
                names.Add(found.ToString());

            // Assert
            Assert.Contains(keyName, names);
        }

        [Fact]
        public async Task ShouldRemoveAllKeys()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output, TestTools.ConfigureRedisAdmin);
            var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
            await key.SetAsync("value");
            IRedisServer server = redis.Service.Server();

            // Act
            await server.FlushAllAsync();
            var exists = await key.ExistsAsync();

            // Assert
            Assert.False(exists);
        }
    }
}
