using MyLab.Redis.Keys;
using Xunit.Abstractions;

namespace IntegrationTests.Keys
{
    public class ListRedisKeyBehavior(ITestOutputHelper output)
    {
        [Fact]
        public async Task ShouldReturnElementAtIndex()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);
            await key.RightPushAsync(["a", "b", "c"]);

            // Act
            var value = await key.GetByIndexAsync(1);

            // Assert
            Assert.Equal("b", value.ToString());
        }

        [Fact]
        public async Task ShouldInsertElementAfterPivot()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);
            await key.RightPushAsync(["a", "c"]);

            // Act
            var length = await key.InsertAfterAsync("a", "b");

            // Assert
            Assert.Equal(3, length);
        }

        [Fact]
        public async Task ShouldInsertElementBeforePivot()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);
            await key.RightPushAsync(["a", "c"]);

            // Act
            var length = await key.InsertBeforeAsync("c", "b");

            // Assert
            Assert.Equal(3, length);
        }

        [Fact]
        public async Task ShouldRemoveFirstElement()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);
            await key.RightPushAsync(["a", "b"]);

            // Act
            var value = await key.LeftPopAsync();

            // Assert
            Assert.Equal("a", value.ToString());
        }

        [Fact]
        public async Task ShouldAddElementAtHead()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);

            // Act
            var length = await key.LeftPushAsync("a");

            // Assert
            Assert.Equal(1, length);
        }

        [Fact]
        public async Task ShouldAddSeveralElementsAtHead()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);

            // Act
            var length = await key.LeftPushAsync(["a", "b"]);

            // Assert
            Assert.Equal(2, length);
        }

        [Fact]
        public async Task ShouldReturnElementCount()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);
            await key.RightPushAsync(["a", "b", "c"]);

            // Act
            var length = await key.LengthAsync();

            // Assert
            Assert.Equal(3, length);
        }

        [Fact]
        public async Task ShouldReturnElementRange()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);
            await key.RightPushAsync(["a", "b", "c"]);

            // Act
            var values = await key.RangeAsync(0, 1);

            // Assert
            Assert.Equal(["a", "b"], values.Select(value => value.ToString()).ToArray());
        }

        [Fact]
        public async Task ShouldRemoveMatchingElements()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);
            await key.RightPushAsync(["a", "b", "a"]);

            // Act
            var removed = await key.RemoveElementAsync("a");

            // Assert
            Assert.Equal(2, removed);
        }

        [Fact]
        public async Task ShouldRemoveLastElement()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);
            await key.RightPushAsync(["a", "b"]);

            // Act
            var value = await key.RightPopAsync();

            // Assert
            Assert.Equal("b", value.ToString());
        }

        [Fact]
        public async Task ShouldAddElementAtTail()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);

            // Act
            var length = await key.RightPushAsync("a");

            // Assert
            Assert.Equal(1, length);
        }

        [Fact]
        public async Task ShouldAddSeveralElementsAtTail()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);

            // Act
            var length = await key.RightPushAsync(["a", "b"]);

            // Assert
            Assert.Equal(2, length);
        }

        [Fact]
        public async Task ShouldReplaceElementAtIndex()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);
            await key.RightPushAsync(["a", "b"]);

            // Act
            await key.SetByIndexAsync(1, "c");
            var value = await key.GetByIndexAsync(1);

            // Assert
            Assert.Equal("c", value.ToString());
        }

        [Fact]
        public async Task ShouldKeepSpecifiedRange()
        {
            // Arrange
            await using var redis = await TestTools.OpenRedisAsync(output);
            var key = new ListRedisKey(redis.NewKeyName(), redis.Database);
            await key.RightPushAsync(["a", "b", "c"]);

            // Act
            await key.TrimAsync(0, 1);
            var values = await key.RangeAsync();

            // Assert
            Assert.Equal(["a", "b"], values.Select(value => value.ToString()).ToArray());
        }
    }
}
