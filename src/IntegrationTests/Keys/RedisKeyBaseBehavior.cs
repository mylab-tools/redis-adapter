using System.Text;
using MyLab.Redis.Keys;
using StackExchange.Redis;
using Xunit.Abstractions;

namespace IntegrationTests.Keys;

public class RedisKeyBaseBehavior(ITestOutputHelper output)
{
    [Fact]
    public async Task ShouldRemoveKey()
    {
        // Arrange
        await using var redis = await TestTools.OpenRedisAsync(output);
        var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
        await key.SetAsync("value");

        // Act
        await key.DeleteAsync();
        var exists = await key.ExistsAsync();

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task ShouldReturnSerializedValue()
    {
        // Arrange
        await using var redis = await TestTools.OpenRedisAsync(output);
        var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
        await key.SetAsync("value");

        string? stringDump = null;

        // Act
        var dump = await key.DumpAsync();

        if(dump != null)
            stringDump = Encoding.UTF8.GetString(dump);

        // Assert
        Assert.Contains("value", stringDump);
    }

    [Fact]
    public async Task ShouldReportExistingKey()
    {
        // Arrange
        await using var redis = await TestTools.OpenRedisAsync(output);
        var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
        await key.SetAsync("value");

        // Act
        var exists = await key.ExistsAsync();

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ShouldReportExistingKeyWhenDoesNotExists()
    {
        // Arrange
        await using var redis = await TestTools.OpenRedisAsync(output);
        var key = new StringRedisKey(redis.NewKeyName(), redis.Database);

        // Act
        var exists = await key.ExistsAsync();

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task ShouldSetRelativeExpiration()
    {
        // Arrange
        await using var redis = await TestTools.OpenRedisAsync(output);
        var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
        await key.SetAsync("value");

        // Act
        var set = await key.SetExpirationAsync(TimeSpan.FromMinutes(5));

        // Assert
        Assert.True(set);
    }

    [Fact]
    public async Task ShouldSetAbsoluteExpiration()
    {
        // Arrange
        await using var redis = await TestTools.OpenRedisAsync(output);
        var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
        await key.SetAsync("value");

        // Act
        var set = await key.SetExpirationAsync(DateTime.UtcNow.AddMinutes(30));

        // Assert
        Assert.True(set);
    }

    [Fact]
    public async Task ShouldReturnExpirationMoment()
    {
        // Arrange
        await using var redis = await TestTools.OpenRedisAsync(output);
        var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
        await key.SetAsync("value");
        var expiry = DateTime.UtcNow.AddMinutes(30);
        await key.SetExpirationAsync(expiry);

        // Act
        var actual = await key.GetExpirationAsync();

        // Assert
        Assert.NotNull(actual);
        Assert.InRange(actual.Value, expiry.ToLocalTime().AddSeconds(-2), expiry.ToLocalTime().AddSeconds(2));
    }

    [Fact]
    public async Task ShouldReturnIdleTime()
    {
        // Arrange
        await using var redis = await TestTools.OpenRedisAsync(output);
        var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
        await key.SetAsync("value");

        // Act
        var idle = await key.GetIdleTimeAsync();

        // Assert
        Assert.NotNull(idle);
    }

    [Fact]
    public async Task ShouldClearExpiration()
    {
        // Arrange
        await using var redis = await TestTools.OpenRedisAsync(output);
        var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
        await key.SetAsync("value");
        await key.SetExpirationAsync(TimeSpan.FromMinutes(5));

        // Act
        var cleared = await key.PersistAsync();
        var ttl = await key.GetTtlAsync();

        // Assert
        Assert.True(cleared);
        Assert.Null(ttl);
    }

    [Fact]
    public async Task ShouldReturnRemainingLifetime()
    {
        // Arrange
        await using var redis = await TestTools.OpenRedisAsync(output);
        var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
        await key.SetAsync("value");
        await key.SetExpirationAsync(TimeSpan.FromMinutes(5));

        // Act
        var ttl = await key.GetTtlAsync();

        // Assert
        Assert.NotNull(ttl);
        Assert.InRange(ttl.Value.TotalSeconds, 1, 300);
    }

    [Fact]
    public async Task ShouldReportStringType()
    {
        // Arrange
        await using var redis = await TestTools.OpenRedisAsync(output);
        var key = new StringRedisKey(redis.NewKeyName(), redis.Database);
        await key.SetAsync("value");

        // Act
        var type = await key.GetRedisTypeAsync();

        // Assert
        Assert.Equal(RedisType.String, type);
    }
}