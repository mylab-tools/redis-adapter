using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyLab.Redis;
using MyLab.Redis.Services;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class IntegrationBehavior
    {
        private readonly ITestOutputHelper _output;

        public IntegrationBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldIntegrateRedisService()
        {
            //Arrange
            var serviceCollection = new ServiceCollection();
            
            //Act
            serviceCollection.AddRedis(RedisConnectionStrategy.Lazy);
            serviceCollection.ConfigureRedis(TestTools.ConfigureOptions);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var service = ActivatorUtilities.CreateInstance<PingService>(serviceProvider);
            var latency = await service.PingAsync();

            _output.WriteLine($"Ping latency: {latency}");

            //Assert
            Assert.True(latency.Ticks > 0);
        }

        class PingService
        {
            private readonly IRedisService _redis;

            public PingService(IRedisService redis)
            {
                _redis = redis;
            }

            public Task<TimeSpan> PingAsync()
            {
                return _redis.Server().PingAsync();
            }
        }
    }
}
