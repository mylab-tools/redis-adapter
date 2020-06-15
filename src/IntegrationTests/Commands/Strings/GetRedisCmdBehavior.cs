using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Strings;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Strings
{
    
    public class GetRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public GetRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldGetKeyValue()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var setCmd = new SetRedisCmd(key, "foo");
            await setCmd.PerformAsync(c);

            var getCmd = new GetRedisCmd(key);

            //Act
            var restStr = await getCmd.PerformAsync(c);

            //Assert
            Assert.Equal("foo", restStr);
        }

        [Fact]
        public async Task ShouldGetCyrillicKeyValue()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var setCmd = new SetRedisCmd(key, "абвгодеёжзийклмнопрстуфхцчшщъыьэюя");
            await setCmd.PerformAsync(c);

            var getCmd = new GetRedisCmd(key);

            //Act
            var restStr = await getCmd.PerformAsync(c);

            //Assert
            Assert.Equal("абвгодеёжзийклмнопрстуфхцчшщъыьэюя", restStr);
        }

        [Fact]
        public async Task ShouldGetBigData()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var bigData = string.Join("", Enumerable.Repeat(Guid.NewGuid().ToString("N"), 15000));

            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var setCmd = new SetRedisCmd(key, bigData);
            await setCmd.PerformAsync(c);

            var getCmd = new GetRedisCmd(key);

            //Act
            var restStr = await getCmd.PerformAsync(c);

            //Assert
            Assert.Equal(bigData, restStr);
        }
    }
}
