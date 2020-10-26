using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyLab.Redis;

namespace IntegrationTests
{
    static class TestTools
    {
        public const string Cache100MsName = "100msCache";
        public const string Cache1000MsName = "1000msCache";
        public const string Cache1MinName = "1minCache";
        public const string CacheDefaultName = Cache100MsName;

        public static RedisOptions Options => new RedisOptions()
        {
            ConnectionString = "localhost:9110,allowAdmin=true",
            Cache = new []
            {
                new CacheOptions
                {
                    Name = Cache100MsName,
                    DefaultExpiry = TimeSpan.FromMilliseconds(100).ToString(),
                    Key = "cache:" + Cache100MsName
                },
                new CacheOptions
                {
                    Name = Cache1000MsName,
                    DefaultExpiry = TimeSpan.FromMilliseconds(1000).ToString(),
                    Key = "cache:" + Cache1000MsName
                },
                new CacheOptions
                {
                    Name = Cache1MinName,
                    DefaultExpiry = TimeSpan.FromMinutes(1).ToString(),
                    Key = "cache:" + Cache1MinName
                },
            }

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
            return "foo_" + Guid.NewGuid().ToString("N");
        }

        public static async Task PerformTest(TestInvocation testAct)
        {
            var redis = CreateRedisManager();

            try
            {
                await testAct(redis, NewKeyName());
            }
            catch
            {
                await redis.Server().FlushDatabaseAsync();
                throw;
            }
        }
    }

    delegate Task TestInvocation(IRedisService redis, string testKey);
}