using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.List;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.List
{
    
    public class ListLeftPushRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public ListLeftPushRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldPushItems()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var items = new[] {"foo", "bar"};
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());

            var cmd = new ListLeftPushRedisCmd(key);
            cmd.Items.AddRange(items);

            //Act
            var intResp = await cmd.PerformAsync(c);

            //Assert
            Assert.Equal(items.Length, intResp);
        }

        [Fact]
        public async Task ShouldPushItemsAtTheStart()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var initialItems = new[] { "foo", "bar" };
            var additionItems = new[] { "baz", "bat" };
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());

            var pushInitCmd = new ListLeftPushRedisCmd(key);
            pushInitCmd.Items.AddRange(initialItems);
            await pushInitCmd.PerformAsync(c);

            var pushAddCmd = new ListLeftPushRedisCmd(key);
            pushAddCmd.Items.AddRange(additionItems);

            //Act
            await pushAddCmd.PerformAsync(c);


            var getCmd = new ListRangeRedisCmd(key);
            var listItems = await getCmd.PerformAsync(c);


            //Assert
            var expectedItems = additionItems.Reverse().Concat(initialItems.Reverse());

            Assert.Equal(listItems, expectedItems);
        }
    }
}
