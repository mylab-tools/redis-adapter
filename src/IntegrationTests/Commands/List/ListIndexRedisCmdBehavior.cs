using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.List;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.List
{
    public class ListIndexRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public ListIndexRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldProvideItemByIndex()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var items = new[] {"foo", "bar", "baz"};
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());

            var cmd = new ListLeftPushRedisCmd(key);
            cmd.Items.AddRange(items);
            await cmd.PerformAsync(c);

            var indexCmd = new ListIndexRedisCmd(key)
            {
                Index = 1
            };

            //Act
            var item = await indexCmd.PerformAsync(c);

            //Assert
            Assert.Equal("bar", item);
        }
    }
}
