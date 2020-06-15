using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Keys;
using MyLab.Redis.Commands.Strings;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Keys
{
    public class DeleteKeyRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public DeleteKeyRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldDeleteKeys()
        {
            //Arrange
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var setCmd = new SetRedisCmd(key, "foo");
            using var c = await _fixture.Source.ProvideConnectionAsync();

            await setCmd.PerformAsync(c);

            var deleteCmd = new DeleteKeyRedisCmd(key);

            //Act
            await deleteCmd.PerformAsync(c);

            var getCmd = new GetRedisCmd(key);
            var resp = await getCmd.PerformAsync(c);

            //Assert
            Assert.Null(resp);
        }
        
        [Fact]
        public async Task ShouldReturnDeletedKeyCount()
        {
            //Arrange
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var setCmd = new SetRedisCmd(key, "foo");
            using var c = await _fixture.Source.ProvideConnectionAsync();

            await setCmd.PerformAsync(c);

            var deleteCmd = new DeleteKeyRedisCmd(key);

            //Act
            var delRes = await deleteCmd.PerformAsync(c);

            //Assert
            Assert.Equal(1, delRes);
        }
    }
}
