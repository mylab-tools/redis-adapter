using MyLab.Redis.Keys;
using Xunit.Abstractions;

namespace IntegrationTests.Keys
{
    public class SortedRedisKeyBehavior(ITestOutputHelper output)
    {
        [Fact]
        public async Task ShouldReturnElementsInSortedOrder()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);
            await key.RightPushAsync(["10", "2", "1"]);

            // Act
            var values = await key.SortAsync();

            // Assert
            Assert.Equal(["1", "2", "10"], values.Select(value => value.ToString()).ToArray());
        }

        [Fact]
        public async Task ShouldStoreSortedElementsInDestination()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);
            var destination = new ListRedisKey(redis.NewKeyName(), redis.Database);
            await key.RightPushAsync(["10", "2", "1"]);

            // Act
            var stored = await key.SortAndStoreAsync(destination.KeyName);

            // Assert
            Assert.Equal(3, stored);
        }
    }
}
