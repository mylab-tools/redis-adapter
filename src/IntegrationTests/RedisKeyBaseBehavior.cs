using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class RedisKeyBaseBehavior
    {
        [Fact]
        public async Task ShouldSetExpiry()
        {
            //Arrange
            var redis = TestTools.CreateRedisManager();

            var fooKey = redis.Keys().String("foo");
            await fooKey.SetAsync("bar");

            //Act
            await fooKey.ExpireAsync(TimeSpan.FromMilliseconds(100));

            await Task.Delay(150);
            var exists = await fooKey.ExistsAsync();

            //Assert
            Assert.False(exists);
        }
    }
}
