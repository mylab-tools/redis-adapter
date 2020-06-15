using System.Threading.Tasks;
using MyLab.Redis.Commands.Connection;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Connection
{
    public class EchoRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public EchoRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Theory]
        [InlineData("Foo message")]
        [InlineData("Фуу сообщение")]
        [InlineData("foo")]
        public async Task ShouldSendMessage(string message)
        {
            //Arrange
            var cmd = new EchoRedisCmd(message);
            using (var c = await _fixture.Source.ProvideConnectionAsync())
            {
                //Act
                var resp = await cmd.PerformAsync(c);

                //Assert
                Assert.Equal(message, resp);
            }
        }
    }
}
