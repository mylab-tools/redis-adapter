using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyLab.Redis;

namespace IntegrationTests
{
    static class TestTools
    {
        public static RedisOptions Options => new RedisOptions()
        {
            ConnectionString = "localhost:9110,allowAdmin=true"
        };

        public static IRedisService CreateRedisManager()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddRedisService(Options);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return (IRedisService) serviceProvider.GetService(typeof(IRedisService));
        }

        public static string NewKeyName()
        {
            return "new_" + Guid.NewGuid().ToString("N");
        }

        public static async Task PerformTest(Func<IRedisService, string, Task> testAct)
        {
            var redis = CreateRedisManager();

            try
            {
                await testAct(redis, NewKeyName());
            }
            catch (Exception e)
            {
                await redis.Server().FlushDatabaseAsync();
            }
        }
    }
}