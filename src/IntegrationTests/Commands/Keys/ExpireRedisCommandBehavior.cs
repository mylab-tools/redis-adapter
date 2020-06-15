using System;
using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Keys;
using MyLab.Redis.Commands.Strings;
using MyLab.Redis.Connection;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Keys
{
    public class ExpireRedisCommandBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public ExpireRedisCommandBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldAssignExpirationToKey()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());

            var setCmd = new SetRedisCmd(key, "foo");
            await setCmd.PerformAsync(c);

            //Act
            var expCmd = new ExpireRedisCommand(key, TimeSpan.FromMilliseconds(100));
            await expCmd.PerformAsync(c);

            var keyVal1 = await GetKeyValue(c);

            await Task.Delay(500);

            var keyVal2 = await GetKeyValue(c);

            //Assert
            Assert.NotNull(keyVal1);
            Assert.Null(keyVal2);

            async Task<string> GetKeyValue(IRedisConnection conn)
            {
                var getCmd = new GetRedisCmd(key);
                return await getCmd.PerformAsync(conn);
            }
        }

        [Fact]
        public async Task ShouldReturnRightResults()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());

            var setCmd = new SetRedisCmd(key, "foo");
            await setCmd.PerformAsync(c);

            //Act
            var expCmd = new ExpireRedisCommand(key, TimeSpan.FromMilliseconds(500));
            var setExpirySucc = await expCmd.PerformAsync(c);

            var expCmdForNotExist = new ExpireRedisCommand(Guid.NewGuid().ToString(), TimeSpan.FromMilliseconds(500));
            var resForNotExist = await expCmdForNotExist.PerformAsync(c);

            //Assert
            Assert.True(setExpirySucc);
            Assert.False(resForNotExist);
        }
    }
}
