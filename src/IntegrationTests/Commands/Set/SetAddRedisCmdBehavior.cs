using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Set;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Set
{
    public class SetAddRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public SetAddRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldSaveValue()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();

            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var cmd = new SetAddRedisCmd(key);
            cmd.Members.Add("Foo");

            //Act & Assert
            await cmd.PerformAsync(c);
        }
    }
}
