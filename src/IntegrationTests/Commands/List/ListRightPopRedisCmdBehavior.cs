using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.List;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.List
{
    
    public class ListRightPopRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public ListRightPopRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldPopItemFromEnd()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var items = new[] { "foo", "bar" };
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());

            var pushInitCmd = new ListRightPushRedisCmd(key);
            pushInitCmd.Items.AddRange(items);
            await pushInitCmd.PerformAsync(c);

            var rPopCmd = new ListRightPopRedisCmd(key);
            
            //Act
            var popedItem = await rPopCmd.PerformAsync(c);

            var getCmd = new ListRangeRedisCmd(key);
            var listItems = await getCmd.PerformAsync(c);
            
            //Assert
            Assert.Equal("bar", popedItem);

            Assert.Single(listItems);
            Assert.Equal("foo", listItems[0]);
        }
    }
}
