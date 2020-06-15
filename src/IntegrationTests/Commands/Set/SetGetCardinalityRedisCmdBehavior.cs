using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Set;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Set
{
    public class SetGetCardinalityRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public SetGetCardinalityRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldProvideSetCardinality()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var addCmd = new SetAddRedisCmd(key);
            addCmd.Members.Add("Foo");
            addCmd.Members.Add("Bar");

            await addCmd.PerformAsync(c);

            var cardinalityCmd = new SetGetCardinalityRedisCmd(key);

            //Act
            var cardinality = await cardinalityCmd.PerformAsync(c);

            //Assert
            Assert.Equal(2, cardinality);
        }
    }
}