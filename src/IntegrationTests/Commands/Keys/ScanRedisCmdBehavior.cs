using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Keys;
using MyLab.Redis.Commands.Strings;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Keys
{
    
    public class ScanRedisCmdBehavior : IAsyncLifetime, IClassFixture<RedisConnectionFixture>
    {
        private static string[] TestKeys;

        private readonly RedisConnectionFixture _fixture;

        public ScanRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }
        
        [Fact]
        public async Task ShouldSearchKeysByPatternWithCountLimit()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var cmd = new ScanRedisCmd(0)
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
            var cmd1 = new ScanRedisCmd(0) { Count = 3 };
            var result1 = await cmd1.PerformAsync(c);
            
            var cmd2 = new ScanRedisCmd(result1.NewCursor) { Count = 3 };
            
            //Act
            var result2 = await cmd2.PerformAsync(c);

            //Assert
            Assert.Equal(result1.Items, result2.Items);
        }

        public async Task InitializeAsync()
        {
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var testKey = _fixture.NewKey(MethodBase.GetCurrentMethod());

            var keys = new List<string>();
            for (int i = 0; i < 20; i++)
                keys.Add(testKey + "_" + (i % 2 == 0 ? "even" : "odd") + "_" + i);
            TestKeys = keys.ToArray();

            foreach (var key in TestKeys)
            {
                var setCmd = new SetRedisCmd(key, "foo");
                await setCmd.PerformAsync(c);
            }
        }

        public async Task DisposeAsync()
        {
            using var c = await _fixture.Source.ProvideConnectionAsync();
            foreach (var key in TestKeys)
            {
                var delCmd = new DeleteKeyRedisCmd(key);
                await delCmd.PerformAsync(c);
            }
        }
    }
}
