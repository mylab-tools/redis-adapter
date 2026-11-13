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
        var services = new ServiceCollection();

        services
            .AddLogging(c => c
                .AddMyLabConsole()
                .AddXUnit(output)
            )
            .AddRedis()
            .ConfigureRedis(TestTools.ConfigureRedis);

        await using var serviceProvider = services.BuildServiceProvider();
        var hostedServices = serviceProvider.GetServices<IHostedService>().ToArray();

        bool connected;

        // Act
        try
        {
                
            foreach (var hostedService in hostedServices)
                await hostedService.StartAsync(CancellationToken.None);

            var redisService = serviceProvider.GetRequiredService<IRedisService>();
            connected = redisService.Connected;
        }
        finally
        {
            foreach (var hostedService in hostedServices)
                await hostedService.StopAsync(CancellationToken.None);
        }

        // Assert
        Assert.True(connected);
    }

    [Fact]
    public async Task ShouldNotBeConnectedWhenNotConfig()
    {
        // Arrange
        var services = new ServiceCollection();

        services
            .AddLogging(c => c
                .AddMyLabConsole()
                .AddXUnit(output)
            )
            .AddRedis();
            //.ConfigureRedis(TestTools.ConfigureRedis);

        await using var serviceProvider = services.BuildServiceProvider();
        var hostedServices = serviceProvider.GetServices<IHostedService>().ToArray();

        bool connected;

        // Act
        try
        {

            foreach (var hostedService in hostedServices)
                await hostedService.StartAsync(CancellationToken.None);

            var redisService = serviceProvider.GetRequiredService<IRedisService>();
            connected = redisService.Connected;
        }
        finally
        {
            foreach (var hostedService in hostedServices)
                await hostedService.StopAsync(CancellationToken.None);
        }

        // Assert
        Assert.False(connected);
    }
}