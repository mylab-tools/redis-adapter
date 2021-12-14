using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class StringKeyBehavior
    {
        private readonly ITestOutputHelper _output;

        public StringKeyBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldSetAndGetValue()
        {
            //Arrange   
            var redis = TestTools.CreateRedisService(_output);
            var key = redis.Db().String("foo");

            //Act
            await key.SetAsync("bar");

            var val = await key.GetAsync();
            await key.DeleteAsync();

            //Assert
            Assert.Equal("bar", val);
        }
    }
}
