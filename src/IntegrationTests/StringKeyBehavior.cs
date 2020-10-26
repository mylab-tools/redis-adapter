using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class StringKeyBehavior
    {
        [Fact]
        public async Task ShouldSetAndGetValue()
        {
            //Arrange   
            var redis = TestTools.CreateRedisManager();
            var key = redis.Keys().String("foo");

            //Act
            await key.SetAsync("bar");

            var val = await key.GetAsync();
            await key.DeleteAsync();

            //Assert
            Assert.Equal("bar", val);
        }
    }
}
