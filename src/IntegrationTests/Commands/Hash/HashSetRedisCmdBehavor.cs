using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Hash;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Hash
{
    public class HashSetRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public HashSetRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldSaveValue()
        {
            //Arrange
            var controlValue = "Foo";
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var cmd = new HashSetRedisCmd(key, "field", controlValue);

            using var c = await _fixture.Source.ProvideConnectionAsync();

            //Act & Assert
            await cmd.PerformAsync(c);
        }
    }
}
