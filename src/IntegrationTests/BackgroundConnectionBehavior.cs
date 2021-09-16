using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyLab.Redis;
using MyLab.Redis.Services;
using StackExchange.Redis;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class BackgroundConnectionBehavior
    {
        private readonly ITestOutputHelper _output;

        public BackgroundConnectionBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldConnect()
        {
            //Arrange
            var host = new HostBuilder()
                .ConfigureServices(srv => srv
                    .AddRedis(RedisConnectionStrategy.Background)
                    .ConfigureRedis(TestTools.ConfigureOptions)
                    .AddLogging(l => l.AddXUnit(_output))
                    )
                .Build();

            var redisService = (IRedisService)host.Services.GetService(typeof(IRedisService));
            var connManager = (IBackgroundRedisConnectionManager)host.Services.GetService(typeof(IBackgroundRedisConnectionManager));

            var ev = new ManualResetEvent(false);

            connManager.Connected += (sender, args) =>
            {
                ev.Set();
            };
            
            RedisValue echoResult;

            try
            {
                //Act
                await host.StartAsync();

                var eventOccurred = ev.WaitOne(TimeSpan.FromSeconds(3));
                                if(!eventOccurred) throw new TimeoutException("Test connection timeout");

                echoResult = await redisService.Server().EchoAsync("foo");
            }
            finally
            {
                await host.StopAsync();
                host.Dispose();
            }

            //Assert
            Assert.Equal("foo", echoResult.ToString());
        }
    }
}
