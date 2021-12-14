using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class ServerToolsBehavior
    {
        private readonly ITestOutputHelper _output;

        public ServerToolsBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldSelectDb()
        {
            //Arrange
            var redis = TestTools.CreateRedisService(_output);

            var keyName = TestTools.NewKeyName();

            var fooKey = redis.Db(0).String(keyName);
            await fooKey.SetAsync("foo");
            await fooKey.ExpireAsync(TimeSpan.FromSeconds(10));

            //Act
            var db0Value = await redis.Db(0).String(keyName).GetAsync();
            var db1Value = await redis.Db(1).String(keyName).GetAsync();

            //Assert
            Assert.Equal("foo", db0Value.ToString());
            Assert.True( db1Value.IsNull);
        }

        [Fact]
        public async Task ShouldFlushDb()
        {
            //Arrange
            var redis = TestTools.CreateRedisService(_output);
            var fooKey = redis.Db().String(TestTools.NewKeyName());
            await fooKey.SetAsync("bar");
            await fooKey.ExpireAsync(TimeSpan.FromSeconds(10));

            //Act
            await redis.Server().FlushDatabaseAsync();

            var exists = await fooKey.ExistsAsync();

            //Assert
            Assert.False(exists);
        }
    }
}
