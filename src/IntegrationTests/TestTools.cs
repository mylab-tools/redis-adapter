using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyLab.Redis.Options;

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

        public static async Task StartHosted(IServiceProvider sp)
        {
            var hostedServices = sp.GetServices<IHostedService>().ToArray();
            foreach (var hostedService in hostedServices)
                await hostedService.StartAsync(CancellationToken.None);
        }

        public static async Task StopHosted(IServiceProvider sp)
        {
            var hostedServices = sp.GetServices<IHostedService>().ToArray();
            foreach (var hostedService in hostedServices)
                await hostedService.StopAsync(CancellationToken.None);
        }
    }
}
