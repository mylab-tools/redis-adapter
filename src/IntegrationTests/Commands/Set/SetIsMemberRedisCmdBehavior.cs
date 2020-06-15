using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Set;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Set
{
    public class SetIsMemberRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public SetIsMemberRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldCheckValue()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var setCmd = new SetAddRedisCmd(key);
            setCmd.Members.Add("Foo");

            var checkCmd = new SetIsMemberRedisCmd(key, "Foo");


            //Act
            await setCmd.PerformAsync(c);
            var respFlag = await checkCmd.PerformAsync(c);

            //Assert
            Assert.True(respFlag);
        }
    }
}
