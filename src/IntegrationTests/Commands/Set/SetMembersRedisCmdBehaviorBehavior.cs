using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Set;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Set
{
    
    public class SetMembersRedisCmdBehaviorBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public SetMembersRedisCmdBehaviorBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldGetAllMembers()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var members = new[] {"Foo", "Bar"};
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());

            var addCmd = new SetAddRedisCmd(key);
            addCmd.Members.AddRange(members);

            await addCmd.PerformAsync(c);

            var membersCmd = new SetMembersRedisCmd(key);

            //Act
            var items = await membersCmd.PerformAsync(c);

            //Assert
            Assert.Equal(members, items);
        }
    }
}
