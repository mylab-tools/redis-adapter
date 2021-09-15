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

            var fooKey = redis.Db().String("foo");
            await fooKey.SetAsync("bar");

            //Act
            await fooKey.ExpireAsync(TimeSpan.FromSeconds(0.5));

            var exists1 = await fooKey.ExistsAsync();

            await Task.Delay(TimeSpan.FromSeconds(1));

            var exists2 = await fooKey.ExistsAsync();

            //Assert
            Assert.True(exists1);
            Assert.False(exists2);
        }
    }
}
