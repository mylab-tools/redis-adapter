using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyLab.Log;
using MyLab.Log.XUnit;
using Xunit.Abstractions;

namespace IntegrationTests.HealthCheck
{
    public class RedisHealthCheckBehavior(ITestOutputHelper output)
    {
        [Fact]
        public async Task ShouldBeHealthyWhenRedisIsConnected()
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

            services
                .AddHealthChecks()
                .AddRedis();

            await using var serviceProvider = services.BuildServiceProvider();
            await TestTools.StartHosted(serviceProvider);

            var healthCheckService = serviceProvider.GetRequiredService<HealthCheckService>();
            HealthReport report;

            // Act
            try
            {
                report = await healthCheckService.CheckHealthAsync();
            }
            finally
            {
                await TestTools.StopHosted(serviceProvider);
            }

            // Assert
            Assert.Equal(HealthStatus.Healthy, report.Entries["redis"].Status);
        }

        [Fact]
        public async Task ShouldBeHealthyWhenConnectionIsMissingAndOptional()
        {
            // Arrange
            var services = new ServiceCollection();

            services
                .AddLogging(c => c
                    .AddMyLabConsole()
                    .AddXUnit(output)
                )
                .AddRedis()
                .ConfigureRedis(options =>
                {
                    options.Optional = true;
                    options.ConnectionString = null;
                });

            services
                .AddHealthChecks()
                .AddRedis();

            await using var serviceProvider = services.BuildServiceProvider();
            await TestTools.StartHosted(serviceProvider);

            var healthCheckService = serviceProvider.GetRequiredService<HealthCheckService>();
            HealthReport report;

            // Act
            try
            {
                report = await healthCheckService.CheckHealthAsync();
            }
            finally
            {
                await TestTools.StopHosted(serviceProvider);
            }

            // Assert
            Assert.Equal(HealthStatus.Healthy, report.Entries["redis"].Status);
        }

        [Fact]
        public async Task ShouldBeUnhealthyWhenConnectionIsMissingAndRequired()
        {
            // Arrange
            var services = new ServiceCollection();

            services
                .AddLogging(c => c
                    .AddMyLabConsole()
                    .AddXUnit(output)
                )
                .AddRedis()
                .ConfigureRedis(options =>
                {
                    options.Optional = false;
                    options.ConnectionString = null;
                });

            services
                .AddHealthChecks()
                .AddRedis();

            await using var serviceProvider = services.BuildServiceProvider();
            await TestTools.StartHosted(serviceProvider);

            var healthCheckService = serviceProvider.GetRequiredService<HealthCheckService>();
            HealthReport report;

            // Act
            try
            {
                report = await healthCheckService.CheckHealthAsync();
            }
            finally
            {
                await TestTools.StopHosted(serviceProvider);
            }

            // Assert
            Assert.Equal(HealthStatus.Unhealthy, report.Entries["redis"].Status);
        }
    }
}
