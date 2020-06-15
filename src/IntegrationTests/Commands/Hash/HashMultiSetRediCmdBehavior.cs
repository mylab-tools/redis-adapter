using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Hash;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Hash
{
    public class HashMultiSetRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public HashMultiSetRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldSaveMultiHash()
        {
            //Arrange
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var cmd = new HashMultiSetRedisCmd(key);

            cmd.Fields.Add("prop1", "val1");
            cmd.Fields.Add("prop2", "val2");
            cmd.Fields.Add("prop3", "val3");

            using var c = await _fixture.Source.ProvideConnectionAsync();

            //Act & Assert
            await cmd.PerformAsync(c);
        }
    }
}
