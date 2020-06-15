using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Keys;
using MyLab.Redis.Commands.Strings;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Strings
{
    
    public class DecrRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public DecrRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldDecrementKeyValue()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod()); ;
            var setCmd = new SetRedisCmd(key, "2");
            var incrCmd = new DecrRedisCmd(key);

            await setCmd.PerformAsync(c);

            //Act
            var newValue = await incrCmd.PerformAsync(c);
            
            //Assert
            Assert.Equal(1, newValue);
        }

        [Fact]
        public async Task ShouldSetZeroBeforeOperationWhenKeyNotExists()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var delCmd = new DeleteKeyRedisCmd(key);
            var incrCmd = new DecrRedisCmd(key);

            await delCmd.PerformAsync(c);

            //Act
            var newValue = await incrCmd.PerformAsync(c);

            //Assert
            Assert.Equal(0-1, newValue);
        }
    }
}
