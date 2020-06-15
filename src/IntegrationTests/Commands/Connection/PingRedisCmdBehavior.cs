using System.Threading.Tasks;
using MyLab.Redis.Commands.Connection;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Connection
{
    public class PingRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public PingRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldPingServer()
        {
            //Arrange
            var pingCmd = new PingRedisCmd();
            using var c = await _fixture.Source.ProvideConnectionAsync();

            //Act
            var succ = await pingCmd.PerformAsync(c);
            
            //Assert
            Assert.True(succ);
        }
        
        [Fact]
        public async Task ShouldPingServerSync()
        {
            //Arrange
            var pingCmd = new PingRedisCmd();
            using var c = await _fixture.Source.ProvideConnectionAsync();

            //Act
            var succ = await pingCmd.PerformAsync(c);
            
            //Assert
            Assert.True(succ);
        }
    }
}
