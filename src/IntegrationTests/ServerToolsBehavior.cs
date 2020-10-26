using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class ServerToolsBehavior
    {
        [Fact]
        public async Task ShouldFlushDb()
        {
            //Arrange
            var redis = TestTools.CreateRedisManager();
            var fooKey = redis.Keys().String(TestTools.NewKeyName());
            await fooKey.SetAsync("bar");
            await fooKey.ExpireAsync(TimeSpan.FromSeconds(10));

            //Act
            await redis.Server().FlushDatabaseAsync();

            var exists = await fooKey.ExistsAsync();

            //Assert
            Assert.False(exists);
        }
    }
}
