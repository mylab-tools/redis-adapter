using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.List;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.List
{
    
    public class ListRangeRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public ListRangeRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Theory]
        [InlineData(1, 2, "bar baz")]
        [InlineData(1, -1, "bar baz bat")]
        [InlineData(0, -1, "foo bar baz bat")]
        public async Task ShouldGetRange(int startIndex, int endIndex, string expectedItems)
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var items = new[] {"foo", "bar", "baz", "bat"};
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());

            var cmd = new ListRightPushRedisCmd(key);
            cmd.Items.AddRange(items);
            await cmd.PerformAsync(c);

            var rangeCmd = new ListRangeRedisCmd(key)
            {
                StartIndex = startIndex,
                EndIndex = endIndex
            };

            //Act
            var rangeItems = await rangeCmd.PerformAsync(c);

            //Assert
            Assert.Equal(expectedItems.Split(' '), rangeItems);
        }
    }
}
