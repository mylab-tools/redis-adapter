using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.Redis;
using MyLab.Redis.Connection;
using MyLab.Redis.Services;
using Xunit.Abstractions;

namespace IntegrationTests
{
    static class TestTools
    {
        public static Action<RedisOptions> ConfigureOptions => o =>
        {
            o.ConnectionString = "localhost:9110,allowAdmin=true";
        };

        public static IRedisService CreateRedisService(ITestOutputHelper output, Action<RedisOptions> editOptions = null)
        {
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddRedis(new LazyRedisConnectionPolicy());
            serviceCollection.ConfigureRedis(ConfigureOptions);
            
            if (editOptions != null)
            {
                serviceCollection.ConfigureRedis(editOptions);
            }

            serviceCollection.AddLogging(l => l.AddXUnit(output).AddFilter(f => true));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return (IRedisService) serviceProvider.GetService(typeof(IRedisService));
        }

        public static string NewKeyName()
        {
            return "foo_" + Guid.NewGuid().ToString("N");
        }

        public static async Task PerformTest(ITestOutputHelper output, TestInvocation testAct, Action<RedisOptions> editOptions = null)
        {
            var redis = CreateRedisService(output, editOptions);

            try
            {
                await testAct(redis, NewKeyName());
            }
            catch
            {
                await redis.Server().FlushDatabaseAsync();
                await redis.Db().Script().FlushCacheAsync();
                throw;
            }

            await Task.Delay(100);
        }
    }

    delegate Task TestInvocation(IRedisService redis, string testKey);
}