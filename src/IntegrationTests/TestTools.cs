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
        public const string Cache100MsName = "100msCache";
        public const string Cache1000MsName = "1000msCache";
        public const string Cache1MinName = "1minCache";
        public const string CacheDefaultName = Cache100MsName;

        public static Action<RedisOptions> ConfigureOptions => o =>
        {
            o.ConnectionString = "localhost:9110,allowAdmin=true";
            o.Cache = new[]
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
            };

        };

        public static IRedisService CreateRedisService(ITestOutputHelper output)
        {
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddRedis(new LazyRedisConnectionPolicy());
            serviceCollection.ConfigureRedis(ConfigureOptions);
            serviceCollection.AddLogging(l => l.AddXUnit(output).AddFilter(f => true));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return (IRedisService) serviceProvider.GetService(typeof(IRedisService));
        }

        public static string NewKeyName()
        {
            return "foo_" + Guid.NewGuid().ToString("N");
        }

        public static async Task PerformTest(ITestOutputHelper output, TestInvocation testAct)
        {
            var redis = CreateRedisService(output);

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