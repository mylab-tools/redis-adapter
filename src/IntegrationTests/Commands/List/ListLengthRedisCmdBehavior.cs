using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.List;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.List
{
    
    public class ListLengthRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public ListLengthRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldGetListLength()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var items = new[] {"foo", "bar", "baz"};
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());

            var cmd = new ListLeftPushRedisCmd(key);
            cmd.Items.AddRange(items);
            await cmd.PerformAsync(c);

            var indexCmd = new ListLengthRedisCmd(key);

            //Act
            var length = await indexCmd.PerformAsync(c);

            //Assert
            Assert.Equal(items.Length, length);
        }
    }
}
