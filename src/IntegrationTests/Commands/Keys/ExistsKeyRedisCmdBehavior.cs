using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Keys;
using MyLab.Redis.Commands.Strings;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Keys
{
    public class ExistsKeyRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public ExistsKeyRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldFind()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();

            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var setCmd = new SetRedisCmd(key, "foo");
            await setCmd.PerformAsync(c);

            var existsCms = new ExistsKeyRedisCmd(
                key,
                "not-exists");

            //Act
            var foundCount = await existsCms.PerformAsync(c);
            
            //Assert
            Assert.Equal(1, foundCount);
        }
    }
}
