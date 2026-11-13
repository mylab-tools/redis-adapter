using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using MyLab.Log;
using MyLab.Log.XUnit;
using MyLab.Redis.Options;
using Xunit.Abstractions;

namespace IntegrationTests
{
    static class TestTools
    {
        public static RedisOptions RedisOptions => new()
        {
            ConnectionString = "localhost:16379",
            User = "test",
            Password = "test"
        };

        public static Action<RedisOptions> ConfigureRedis => opt =>
        {
            opt.ConnectionString = RedisOptions.ConnectionString;
            opt.User = RedisOptions.User;
            opt.Password = RedisOptions.Password;
        };

        public static (string Host, int Port)[] RedisEndpoints =>
        [
            ("localhost", 16379),
            ("localhost", 16380),
            ("localhost", 16381)
        ];

        public static RedisOptions ThreeRedisOptions => new()
        {
            ConnectionString = string.Join(",", RedisEndpoints.Select(endpoint => $"{endpoint.Host}:{endpoint.Port}")),
            User = RedisOptions.User,
            Password = RedisOptions.Password
        };

        public static Action<RedisOptions> ConfigureThreeRedis => opt =>
        {
            var source = ThreeRedisOptions;
            opt.ConnectionString = source.ConnectionString;
            opt.User = source.User;
            opt.Password = source.Password;
        };

        public static async Task StartHostedAsync(IServiceProvider sp)
        {
            var hostedServices = sp.GetServices<IHostedService>().ToArray();
            foreach (var hostedService in hostedServices)
                await hostedService.StartAsync(CancellationToken.None);
        }

        public static async Task StopHostedAsync(IServiceProvider sp)
        {
            var hostedServices = sp.GetServices<IHostedService>().ToArray();
            foreach (var hostedService in hostedServices)
                await hostedService.StopAsync(CancellationToken.None);
        }

        public static IServiceCollection CreateAndConfigApp
            (
                Action<RedisOptions> configureOptions,
                ITestOutputHelper output
            )
        {
            var services = new ServiceCollection();

            services
                .AddLogging(c => c
                    .AddMyLabConsole()
                    .AddXUnit(output)
                )
                .AddRedis()
                .ConfigureRedis(configureOptions);

            return services;
        }
    }
}
