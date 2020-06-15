using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Hash;
using MyLab.Redis.Values;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Hash
{
    
    public class HashGetAllRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public HashGetAllRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldGetAllFields()
        {
            //Arrange
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());

            var setCmd = new HashMultiSetRedisCmd(key);
            setCmd.Fields.Add("prop1", "val1");
            setCmd.Fields.Add("prop2", "val2");
            setCmd.Fields.Add("prop3", "val3");

            var getCmd = new HashGetAllRedisCmd(key);

            using var c = await _fixture.Source.ProvideConnectionAsync();

            //Act
            await setCmd.PerformAsync(c);
            var resp = await getCmd.PerformAsync(c);

            //Assert
            Assert.Equal(3, resp.Count);
            Assert.Equal("val1", ((BulkStringRedisValue)resp["prop1"]).Value);
            Assert.Equal("val2", ((BulkStringRedisValue)resp["prop2"]).Value);
            Assert.Equal("val3", ((BulkStringRedisValue)resp["prop3"]).Value);
        }
    }
}
