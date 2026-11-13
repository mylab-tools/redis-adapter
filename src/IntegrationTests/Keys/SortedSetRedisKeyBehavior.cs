using MyLab.Redis.Keys;
using StackExchange.Redis;
using Xunit.Abstractions;

namespace IntegrationTests.Keys
{
    public class SortedSetRedisKeyBehavior(ITestOutputHelper output)
    {
        [Fact]
        public async Task ShouldAddMemberWithScore()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);

            // Act
            var added = await key.AddAsync("a", 1);

            // Assert
            Assert.True(added);
        }

        [Fact]
        public async Task ShouldAddSeveralMembersWithScores()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);

            // Act
            var added = await key.AddAsync([new SortedSetEntry("a", 1), new SortedSetEntry("b", 2)]);

            // Assert
            Assert.Equal(2, added);
        }

        [Fact]
        public async Task ShouldDecreaseMemberScore()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync("a", 5);

            // Act
            var score = await key.DecrementAsync("a", 2);

            // Assert
            Assert.Equal(3, score);
        }

        [Fact]
        public async Task ShouldIncreaseMemberScore()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync("a", 5);

            // Act
            var score = await key.IncrementAsync("a", 2);

            // Assert
            Assert.Equal(7, score);
        }

        [Fact]
        public async Task ShouldReturnMemberCount()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 1), new SortedSetEntry("b", 2)]);

            // Act
            var length = await key.LengthAsync();

            // Assert
            Assert.Equal(2, length);
        }

        [Fact]
        public async Task ShouldReturnCountInLexicographicalRange()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 0), new SortedSetEntry("b", 0), new SortedSetEntry("c", 0)]);

            // Act
            var length = await key.LengthByValueAsync("a", "b");

            // Assert
            Assert.Equal(2, length);
        }

        [Fact]
        public async Task ShouldReturnMembersByRank()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 1), new SortedSetEntry("b", 2), new SortedSetEntry("c", 3)]);

            // Act
            var members = await key.RangeByRankAsync(0, 1);

            // Assert
            Assert.Equal(["a", "b"], members.Select(member => member.ToString()).ToArray());
        }

        [Fact]
        public async Task ShouldReturnMembersAndScoresByRank()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 1), new SortedSetEntry("b", 2)]);

            // Act
            var members = await key.RangeByRankWithScoresAsync();

            // Assert
            Assert.Equal("a", members[0].Element.ToString());
            Assert.Equal(1, members[0].Score);
            Assert.Equal("b", members[1].Element.ToString());
            Assert.Equal(2, members[1].Score);
        }

        [Fact]
        public async Task ShouldReturnMembersByScore()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 1), new SortedSetEntry("b", 2), new SortedSetEntry("c", 3)]);

            // Act
            var members = await key.RangeByScoreAsync(1, 2);

            // Assert
            Assert.Equal(["a", "b"], members.Select(member => member.ToString()).ToArray());
        }

        [Fact]
        public async Task ShouldReturnMembersAndScoresByScore()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 1), new SortedSetEntry("b", 2)]);

            // Act
            var members = await key.RangeByScoreWithScoresAsync(2, 2);

            // Assert
            Assert.Single(members);
            Assert.Equal("b", members[0].Element.ToString());
            Assert.Equal(2, members[0].Score);
        }

        [Fact]
        public async Task ShouldReturnMembersInLexicographicalRange()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 0), new SortedSetEntry("b", 0), new SortedSetEntry("c", 0)]);

            // Act
            var members = await key.RangeByValueAsync("a", "b");

            // Assert
            Assert.Equal(["a", "b"], members.Select(member => member.ToString()).ToArray());
        }

        [Fact]
        public async Task ShouldReturnMembersInLexicographicalRangeInRequestedOrder()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 0), new SortedSetEntry("b", 0), new SortedSetEntry("c", 0)]);

            // Act
            var members = await key.RangeByValueAsync(min: "a", max: "c", exclude: Exclude.None, order: Order.Descending);

            // Assert
            Assert.Equal(["c", "b", "a"], members.Select(member => member.ToString()).ToArray());
        }

        [Fact]
        public async Task ShouldReturnMemberRank()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 1), new SortedSetEntry("b", 2)]);

            // Act
            var rank = await key.RankAsync("b");

            // Assert
            Assert.Equal(1, rank);
        }

        [Fact]
        public async Task ShouldRemoveMember()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync("a", 1);

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
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 1), new SortedSetEntry("b", 2)]);

            // Act
            var removed = await key.RemoveMembersAsync(["a", "b"]);

            // Assert
            Assert.Equal(2, removed);
        }

        [Fact]
        public async Task ShouldRemoveMembersInRankRange()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 1), new SortedSetEntry("b", 2), new SortedSetEntry("c", 3)]);

            // Act
            var removed = await key.RemoveRangeByRankAsync(0, 1);

            // Assert
            Assert.Equal(2, removed);
        }

        [Fact]
        public async Task ShouldRemoveMembersInScoreRange()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 1), new SortedSetEntry("b", 2), new SortedSetEntry("c", 3)]);

            // Act
            var removed = await key.RemoveRangeByScoreAsync(1, 2);

            // Assert
            Assert.Equal(2, removed);
        }

        [Fact]
        public async Task ShouldRemoveMembersInLexicographicalRange()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 0), new SortedSetEntry("b", 0), new SortedSetEntry("c", 0)]);

            // Act
            var removed = await key.RemoveRangeByValueAsync("a", "b");

            // Assert
            Assert.Equal(2, removed);
        }

        [Fact]
        public async Task ShouldEnumerateMatchingMembers()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a1", 1), new SortedSetEntry("b1", 2)]);
            var members = new List<string>();

            // Act
            await foreach (var entry in key.ScanAsync("a*"))
                members.Add(entry.Element.ToString());

            // Assert
            Assert.Equal(["a1"], members);
        }

        [Fact]
        public async Task ShouldReturnMemberScore()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync("a", 1.5);

            // Act
            var score = await key.ScoreAsync("a");

            // Assert
            Assert.Equal(1.5, score);
        }

        [Fact]
        public async Task ShouldRemoveFirstMember()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 1), new SortedSetEntry("b", 2)]);

            // Act
            var entry = await key.PopAsync();

            // Assert
            Assert.Equal("a", entry!.Value.Element.ToString());
            Assert.Equal(1, entry.Value.Score);
        }

        [Fact]
        public async Task ShouldRemoveRequestedNumberOfFirstMembers()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new SortedSetRedisKey(redis.NewKeyName(), redis.Database);
            await key.AddAsync([new SortedSetEntry("a", 1), new SortedSetEntry("b", 2), new SortedSetEntry("c", 3)]);

            // Act
            var entries = await key.PopAsync(2);

            // Assert
            Assert.Equal(["a", "b"], entries.Select(entry => entry.Element.ToString()).ToArray());
        }
    }
}
