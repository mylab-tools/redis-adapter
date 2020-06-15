using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Keys;
using MyLab.Redis.Commands.Set;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Set
{
    
    public class SetScanRedisCmdBehavior : IAsyncLifetime,  IClassFixture<RedisConnectionFixture>
    {
        private static string Key;

        private readonly RedisConnectionFixture _fixture;

        public SetScanRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldSearchKeysByPatternWithCountLimit()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var cmd = new SetScanRedisCmd<string>(0, Key)
            {
                Pattern = "*odd*",
                Count = 3
            };

            //Act
            var result = await cmd.PerformAsync(c);

            //Assert
            Assert.True(result.Items.All(k => k.Contains("odd")));
        }

        [Fact]
        public async Task ShouldSearchNextKeysWithNextCursor()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var cmd1 = new SetScanRedisCmd<string>(0, Key) { Count = 3 };
            var result1 = await cmd1.PerformAsync(c);
            
            var cmd2 = new ScanRedisCmd(result1.NewCursor) { Count = 3 };
            
            //Act
            var result2 = await cmd2.PerformAsync(c);

            //Assert
            Assert.NotEqual(result1.Items, result2.Items);
        }

        public async Task InitializeAsync()
        {
            using var c = await _fixture.Source.ProvideConnectionAsync();
            Key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var setCmd = new SetAddRedisCmd(Key);

            for (int i = 0; i < 20; i++)
                setCmd.Members.Add("foo_" + (i % 2 == 0 ? "even" : "odd"));

            await setCmd.PerformAsync(c);
        }

        public async Task DisposeAsync()
        {
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var delCmd = new DeleteKeyRedisCmd(Key);
            await delCmd.PerformAsync(c);
        }
    }
}
