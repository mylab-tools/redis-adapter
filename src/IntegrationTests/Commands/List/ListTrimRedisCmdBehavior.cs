using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.List;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.List
{
    
    public class ListTrimRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public ListTrimRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Theory]
        [InlineData(1, 2, "bar baz")]
        [InlineData(1, -1, "bar baz bat")]
        [InlineData(0, -1, "foo bar baz bat")]
        public async Task ShouldTrim(int startIndex, int endIndex, string expectedItems)
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var items = new[] { "foo", "bar", "baz", "bat" };
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());

            var cmd = new ListRightPushRedisCmd(key);
            cmd.Items.AddRange(items);
            await cmd.PerformAsync(c);

            var trimCmd = new ListTrimRedisCmd(key)
            {
                StartIndex = startIndex,
                EndIndex = endIndex
            };

            //Act
            await trimCmd.PerformAsync(c);

            var getRangeCmd = new ListRangeRedisCmd(key)
            {
                StartIndex = 0,
                EndIndex = -1
            };

            var rangeItems = await getRangeCmd.PerformAsync(c);

            //Assert
            Assert.Equal(expectedItems.Split(' '), rangeItems);
        }
    }
}
