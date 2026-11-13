using MyLab.Redis.Keys;
using Xunit.Abstractions;

namespace IntegrationTests.Keys
{
    public class SetRedisKeyBehavior(ITestOutputHelper output)
    {
        [Fact]
        public async Task ShouldAddMember()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SetRedisKey(redis.NewKeyName(), redis.Database);

            // Act
            var added = await key.AddAsync("a");

            // Assert
            Assert.True(added);
        }

        [Fact]
        public async Task ShouldAddSeveralMembers()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SetRedisKey(redis.NewKeyName(), redis.Database);

            // Act
            var added = await key.AddAsync(["a", "b"]);

            // Assert
            Assert.Equal(2, added);
        }

        [Fact]
        public async Task ShouldReportMembership()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync("a");

            // Act
            var contains = await key.ContainsAsync("a");

            // Assert
            Assert.True(contains);
        }

        [Fact]
        public async Task ShouldReturnMemberCount()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync(["a", "b"]);

            // Act
            var length = await key.LengthAsync();

            // Assert
            Assert.Equal(2, length);
        }

        [Fact]
        public async Task ShouldReturnAllMembers()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync(["a", "b"]);

            // Act
            var members = await key.MembersAsync();

            // Assert
            Assert.Equal(["a", "b"], members.Select(member => member.ToString()).Order().ToArray());
        }

        [Fact]
        public async Task ShouldMoveMemberToAnotherSet()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var source = new SetRedisKey(redis.NewKeyName(), redis.Database);
            var destination = new SetRedisKey(redis.NewKeyName(), redis.Database);
            await source.AddAsync("a");

            // Act
            var moved = await source.MoveMemberAsync(destination.KeyName, "a");

            // Assert
            Assert.True(moved);
            Assert.False(await source.ContainsAsync("a"));
            Assert.True(await destination.ContainsAsync("a"));
        }

        [Fact]
        public async Task ShouldRemoveRandomMember()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync(["a", "b"]);

            // Act
            var member = await key.PopAsync();

            // Assert
            Assert.Contains(member.ToString(), new[] { "a", "b" });
        }

        [Fact]
        public async Task ShouldRemoveRequestedNumberOfRandomMembers()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync(["a", "b", "c"]);

            // Act
            var members = await key.PopAsync(2);

            // Assert
            Assert.Equal(2, members.Length);
        }

        [Fact]
        public async Task ShouldReturnRandomMember()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync(["a", "b"]);

            // Act
            var member = await key.RandomMemberAsync();

            // Assert
            Assert.Contains(member.ToString(), new[] { "a", "b" });
        }

        [Fact]
        public async Task ShouldReturnSeveralRandomMembers()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync(["a", "b", "c"]);

            // Act
            var members = await key.RandomMembersAsync(2);

            // Assert
            Assert.Equal(2, members.Length);
        }

        [Fact]
        public async Task ShouldRemoveMember()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync("a");

            // Act
            var removed = await key.RemoveMemberAsync("a");

            // Assert
            Assert.True(removed);
        }

        [Fact]
        public async Task ShouldRemoveSeveralMembers()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync(["a", "b"]);

            // Act
            var removed = await key.RemoveMembersAsync(["a", "b"]);

            // Assert
            Assert.Equal(2, removed);
        }

        [Fact]
        public async Task ShouldEnumerateMatchingMembers()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync(["a1", "b1"]);
            var members = new List<string>();

            // Act
            await foreach (var member in key.ScanAsync("a*"))
                members.Add(member.ToString());

            // Assert
            Assert.Equal(["a1"], members);
        }
    }
}
