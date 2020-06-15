using System.Reflection;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Set;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Commands.Set
{
    public class SetRemoveRedisCmdBehaviorBehavior : IClassFixture<RedisConnectionFixture>
    {
        private readonly RedisConnectionFixture _fixture;

        public SetRemoveRedisCmdBehaviorBehavior(RedisConnectionFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldRemoveMembers()
        {
            //Arrange
            using var c = await _fixture.Source.ProvideConnectionAsync();
            var key = _fixture.NewKey(MethodBase.GetCurrentMethod());
            var addCmd = new SetAddRedisCmd(key);
            addCmd.Members.Add("Foo");
            addCmd.Members.Add("Bar");

            await addCmd.PerformAsync(c);

            var remCmd = new SetRemoveRedisCmd(key);
            remCmd.Members.Add("Bar");

            await remCmd.PerformAsync(c);

            var membersCmd = new SetMembersRedisCmd(key);

            //Act
            var items = await membersCmd.PerformAsync(c);

            //Assert
            Assert.Single(items);
            Assert.Equal("Foo", items[0]);
        }
    }
}
