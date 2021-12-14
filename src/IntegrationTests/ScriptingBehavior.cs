using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class ScriptingBehavior
    {
        private readonly ITestOutputHelper _output;

        public ScriptingBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldFlushCache()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var scripting = redis.Db().Script();

                var sha1 = await scripting.LoadAsync("return 10");
                await scripting.FlushCacheAsync();

                //Act
                var exists = await scripting.ExistsAsync(sha1);

                //Assert
                Assert.False(exists);
            });
        }

        [Fact]
        public async Task ShouldLoadScript()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var scripting = redis.Db().Script();

                var sha1 = await scripting.LoadAsync("return 10");

                //Act
                var exists = await scripting.ExistsAsync(sha1);

                //Assert
                Assert.True(exists);
            });
        }

        [Fact]
        public async Task ShouldEvaluateScriptFromCache()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var scripting = redis.Db().Script();

                var sha1 = await scripting.LoadAsync("return 10");

                //Act
                var res = await scripting.BySha(sha1).EvaluateAsync();

                //Assert
                Assert.Equal(10, (int)res);
            });
        }

        [Fact]
        public async Task ShouldEvaluateInlineScript()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var scripting = redis.Db().Script();

                //Act
                var res = await scripting.Inline("return 10").EvaluateAsync();

                //Assert
                Assert.Equal(10, (int)res);
            });
        }

        [Fact]
        public async Task ShouldUseKeys()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var stringKey = redis.Db().String(testKey);
                await stringKey.SetAsync("foo");

                //Act
                var res = await redis.Db().Script()
                    .Inline("return redis.call('get', KEYS[1])")
                    .WithKey(stringKey)
                    .EvaluateAsync();

                //Assert
                Assert.Equal("foo", (string)res);
            });
        }

        [Fact]
        public async Task ShouldUseArgs()
        {
            await TestTools.PerformTest(_output, async (redis, testKey) =>
            {
                //Arrange
                var stringKey = redis.Db().String(testKey);

                //Act
                await redis.Db().Script()
                    .Inline("return redis.call('set', KEYS[1], ARGV[1])")
                    .WithKey(stringKey)
                    .WithArgs("bar")
                    .EvaluateAsync();

                var actualKeyValue = await stringKey.GetAsync();

                //Assert
                Assert.Equal("bar", actualKeyValue);
            });
        }
    }
}
