using System;
using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Keys;
using MyLab.Redis.Commands.Strings;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Strings
{
    public class SetRedisCmdBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public SetRedisCmdBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldSetKayValue()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var cmd = new SetRedisCmd(key, "foo");

            //Act
            bool succ = await cmd.PerformAsync(c);
            
            //Assert
            Assert.True(succ);
        }

        [Fact]
        public async Task ShouldSetKayCyrillicValue()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var cmd = new SetRedisCmd(key, "абвгодеёжзийклмнопрстуфхцчшщъыьэюя");

            //Act
            bool succ = await cmd.PerformAsync(c);

            //Assert
            Assert.True(succ);
        }

        [Fact]
        public async Task ShouldSetExpireTime()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var cmd = new SetRedisCmd(key, "foo")
            {
                ExpireTime = TimeSpan.FromMilliseconds(100)
            };
            var isExistsCmd = new ExistsKeyRedisCmd(key);
            
            //Act
            await cmd.PerformAsync(c);
            await Task.Delay(200);
            var existsCount = await isExistsCmd.PerformAsync(c);
            
            //Assert
            Assert.NotEqual(1, existsCount);
        }
        
        [Fact]
        public async Task ShouldNotSetIfKeyAlreadyExistsAndNxMode()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var cmd1 = new SetRedisCmd(key, "foo");
            var cmd2 = new SetRedisCmd(key, "bar")
            {
                Condition = SetRedisCmd.ConditionMode.OnlyIfNotExists
            };

            //Act
            await cmd1.PerformAsync(c);
            var succ = await cmd2.PerformAsync(c);
            
            //Assert
            Assert.False(succ);
        }
        
        [Fact]
        public async Task ShouldSetIfKeyNotExistsAndNxMode()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var cmd2 = new SetRedisCmd(key, "bar")
            {
                Condition = SetRedisCmd.ConditionMode.OnlyIfNotExists
            };
            
            //Act
            var succ = await cmd2.PerformAsync(c);
            
            //Assert
            Assert.True(succ);
        }
        
        [Fact]
        public async Task ShouldSetIfKeyAlreadyExistsAndXxMode()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var cmd1 = new SetRedisCmd(key, "foo");
            var cmd2 = new SetRedisCmd(key, "bar")
            {
                Condition = SetRedisCmd.ConditionMode.OnlyIfExists
            };
            
            //Act
            await cmd1.PerformAsync(c);
            var succ = await cmd2.PerformAsync(c);
            
            //Assert
            Assert.True(succ);
        }
        
        [Fact]
        public async Task ShouldNotSetIfKeyNotExistsAndXxMode()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var cmd2 = new SetRedisCmd(key, "bar")
            {
                Condition = SetRedisCmd.ConditionMode.OnlyIfExists
            };
            
            //Act
            var succ = await cmd2.PerformAsync(c);
            
            //Assert
            Assert.False(succ);
        }
        
    }
}
