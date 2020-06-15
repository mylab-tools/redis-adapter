using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Hash;
using MyLab.Redis.Values;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Hash
{
    public class HashDeleteRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public HashDeleteRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldDeleteItems()
        {
            //Arrange
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var setFooCmd = new HashSetRedisCmd(key, "fooField", "fooVal");
            var setBarCmd = new HashSetRedisCmd(key, "barField", "barVal");
            using var c = await _fixture.Source.ProvideConnectionAsync();

            await setFooCmd.PerformAsync(c);
            await setBarCmd.PerformAsync(c);

            var deleteCmd = new HashDeleteRedisCmd(key);
            deleteCmd.Fields.Add("fooField");

            //Act
            await deleteCmd.PerformAsync(c);

            var getCmd = new HashGetAllRedisCmd(_fixture.NewKey(MethodBase.GetCurrentMethod()));
            var fields = await getCmd.PerformAsync(c);

            //Assert
            Assert.Single(fields);
            Assert.True(fields.ContainsKey("barField"));
            Assert.Equal("barVal", ((BulkStringRedisValue)fields["barField"]).Value);
        }
    }
}
