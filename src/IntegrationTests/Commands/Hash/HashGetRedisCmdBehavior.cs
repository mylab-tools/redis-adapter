using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Hash;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Hash
{
    
    public class HashGetRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public HashGetRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldGetValue()
        {
            //Arrange
            var controlValue = "Foo";
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var setCmd = new HashSetRedisCmd(key, "field", controlValue);
            var getCmd = new HashGetRedisCmd(key, "field");

            using var c = await _fixture.Source.ProvideConnectionAsync();

            //Act
            await setCmd.PerformAsync(c);
            var resp = await getCmd.PerformAsync(c);

            //Assert

            Assert.Equal("Foo", resp);
        }
    }
}
