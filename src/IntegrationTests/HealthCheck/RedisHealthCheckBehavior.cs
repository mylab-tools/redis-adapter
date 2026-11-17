using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyLab.Redis.HealthCheck;
using Xunit.Abstractions;

namespace IntegrationTests.HealthCheck
{
    public class RedisHealthCheckBehavior(ITestOutputHelper output)
    {
        [Fact]
        public async Task ShouldBeHealthyWhenRedisIsConnected()
        {
            // Arrange
            var services = TestTools.CreateAndConfigApp(TestTools.ConfigureRedis, output);

            services
                .AddHealthChecks()
                .AddRedis();

            await using var serviceProvider = services.BuildServiceProvider();
            await TestTools.StartHostedAsync(serviceProvider);

            var healthCheckService = serviceProvider.GetRequiredService<HealthCheckService>();
            HealthReport report;

            // Act
            try
            {
                report = await healthCheckService.CheckHealthAsync();
            }
            finally
            {
                await TestTools.StopHostedAsync(serviceProvider);
            }

            // Assert
            Assert.Equal(HealthStatus.Healthy, report.Entries["redis"].Status);
        }

        [Fact]
        public async Task ShouldBeHealthyWhenConnectionIsMissingAndOptional()
        {
            // Arrange
            var services = TestTools.CreateAndConfigApp(
                options =>
                {
                    options.Optional = true;
                    options.ConnectionString = null;
                }, output);

            services
                .AddHealthChecks()
                .AddRedis();

            await using var serviceProvider = services.BuildServiceProvider();
            await TestTools.StartHostedAsync(serviceProvider);

            var healthCheckService = serviceProvider.GetRequiredService<HealthCheckService>();
            HealthReport report;

            // Act
            try
            {
                report = await healthCheckService.CheckHealthAsync();
            }
            finally
            {
                await TestTools.StopHostedAsync(serviceProvider);
            }

            // Assert
            Assert.Equal(HealthStatus.Healthy, report.Entries["redis"].Status);
        }

        [Fact]
        public async Task ShouldBeUnhealthyWhenConnectionIsMissingAndRequired()
        {
            // Arrange
            var services = TestTools.CreateAndConfigApp(
                options =>
                {
                    options.Optional = false;
                    options.ConnectionString = null;
                }, output);

            services
                .AddHealthChecks()
                .AddRedis();

            await using var serviceProvider = services.BuildServiceProvider();
            await TestTools.StartHostedAsync(serviceProvider);

            var healthCheckService = serviceProvider.GetRequiredService<HealthCheckService>();
            HealthReport report;

            // Act
            try
            {
                report = await healthCheckService.CheckHealthAsync();
            }
            finally
            {
                await TestTools.StopHostedAsync(serviceProvider);
            }

            // Assert
            Assert.Equal(HealthStatus.Unhealthy, report.Entries["redis"].Status);
        }
    }
}
