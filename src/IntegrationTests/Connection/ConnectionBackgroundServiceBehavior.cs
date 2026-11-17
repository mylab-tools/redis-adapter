using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyLab.Log;
using MyLab.Log.XUnit;
using MyLab.Redis;
using MyLab.Redis.Abstractions;
using Xunit.Abstractions;

namespace IntegrationTests.Connection;

public class ConnectionBackgroundServiceBehavior(ITestOutputHelper output)
{
    [Fact]
    public async Task ShouldBeConnectedWhenHostServicesStart()
    {
        // Arrange
        await using var serviceProvider = TestTools
            .CreateAndConfigApp(TestTools.ConfigureRedis, output)
            .BuildServiceProvider();
        await TestTools.StartHostedAsync(serviceProvider);

        bool connected;

        // Act
        try
        {
            var redisService = serviceProvider.GetRequiredService<IRedisService>();
            connected = redisService.Connected;
        }
        finally
        {
            await TestTools.StopHostedAsync(serviceProvider);
        }

        // Assert
        Assert.True(connected);
    }

    [Fact]
    public async Task ShouldNotBeConnectedWhenNotConfig()
    {
        // Arrange
        await using var serviceProvider = TestTools
            .CreateAndConfigApp(opt =>{}, output)
            .BuildServiceProvider();
        await TestTools.StartHostedAsync(serviceProvider);

        bool connected;

        // Act
        try
        {
            var redisService = serviceProvider.GetRequiredService<IRedisService>();
            connected = redisService.Connected;
        }
        finally
        {
            await TestTools.StopHostedAsync(serviceProvider);
        }

        // Assert
        Assert.False(connected);
    }
}