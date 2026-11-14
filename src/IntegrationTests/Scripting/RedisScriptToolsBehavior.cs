using MyLab.Redis.Scripting;
using Xunit.Abstractions;

namespace IntegrationTests.Scripting
{
    public class RedisScriptToolsBehavior(ITestOutputHelper output)
    {
        const string UnknownSha = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        [Fact]
        public async Task ShouldReturnDigestOfLoadedScript()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            IRedisScriptTools scripts = redis.Service.Db().Script();

            // Act
            var sha = await scripts.LoadAsync("return 1");

            // Assert
            Assert.Matches("^[0-9a-f]{40}$", sha);
        }

        [Fact]
        public async Task ShouldReportCachedScript()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            IRedisScriptTools scripts = redis.Service.Db().Script();
            var sha = await scripts.LoadAsync("return 1");

            // Act
            var exists = await scripts.ExistsAsync(sha);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ShouldReportEachCachedScript()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            IRedisScriptTools scripts = redis.Service.Db().Script();
            var sha = await scripts.LoadAsync("return 1");

            // Act
            var exists = await scripts.ExistsAsync(sha, UnknownSha);

            // Assert
            Assert.Equal([true, false], exists);
        }

        [Fact]
        public async Task ShouldDropCachedScripts()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            IRedisScriptTools scripts = redis.Service.Db().Script();
            var sha = await scripts.LoadAsync("return 1");

            // Act
            await scripts.FlushCacheAsync();
            var exists = await scripts.ExistsAsync(sha);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task ShouldRunInlineScript()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            IRedisScriptTools scripts = redis.Service.Db().Script();

            // Act
            var result = await scripts.Inline("return ARGV[1]").WithArgs("hello").EvaluateAsync();

            // Assert
            Assert.Equal("hello", result.ToString());
        }

        [Fact]
        public async Task ShouldRunLoadedScriptByDigest()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            IRedisScriptTools scripts = redis.Service.Db().Script();
            var sha = await scripts.LoadAsync("return ARGV[1]");

            // Act
            var result = await scripts.BySha(sha).WithArgs("hello").EvaluateAsync();

            // Assert
            Assert.Equal("hello", result.ToString());
        }
    }
}
