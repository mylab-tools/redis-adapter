using Microsoft.Extensions.DependencyInjection;
using MyLab.Redis.Abstractions;
using Xunit.Abstractions;

namespace IntegrationTests.Core
{
    public class RedisServiceBehavior(ITestOutputHelper output)
    {
        [Fact]
        public async Task ShouldReturnAllServersWhenConnectedToThreeRedis()
        {
            // Arrange
            await using var serviceProvider = TestTools
                .CreateAndConfigApp(TestTools.ConfigureThreeRedis, output)
                .BuildServiceProvider();
            await TestTools.StartHostedAsync(serviceProvider);

            var redisService = serviceProvider.GetRequiredService<IRedisService>();
            string[] echoes;

            // Act
            try
            {
                var servers = redisService.GetServers();
                echoes = new string[servers.Length];

                for (var i = 0; i < servers.Length; i++)
                    echoes[i] = (await servers[i].EchoAsync("redis")).ToString();
            }
            finally
            {
                await TestTools.StopHostedAsync(serviceProvider);
            }

            // Assert
            Assert.Equal(3, echoes.Length);
            Assert.All(echoes, echo => Assert.Equal("redis", echo));
        }

        [Fact]
        public async Task ShouldReturnEachServerWhenRequestedByHostAndPort()
        {
            // Arrange
            await using var serviceProvider = TestTools
                .CreateAndConfigApp(TestTools.ConfigureThreeRedis, output)
                .BuildServiceProvider();
            await TestTools.StartHostedAsync(serviceProvider);

            var redisService = serviceProvider.GetRequiredService<IRedisService>();
            var echoes = new List<string>();

            // Act
            try
            {
                foreach (var (host, port) in TestTools.RedisEndpoints)
                {
                    var server = redisService.GetServer(host, port);
                    echoes.Add((await server.EchoAsync("redis")).ToString());
                }
            }
            finally
            {
                await TestTools.StopHostedAsync(serviceProvider);
            }

            // Assert
            Assert.Equal(TestTools.RedisEndpoints.Length, echoes.Count);
            Assert.All(echoes, echo => Assert.Equal("redis", echo));
        }

        [Fact]
        public async Task ShouldReturnServerWhenConnectedToSingleRedis()
        {
            // Arrange
            await using var serviceProvider = TestTools
                .CreateAndConfigApp(TestTools.ConfigureRedis, output)
                .BuildServiceProvider();
            await TestTools.StartHostedAsync(serviceProvider);

            var redisService = serviceProvider.GetRequiredService<IRedisService>();
            string echo;

            // Act
            try
            {
                var server = redisService.GetServer();
                echo = (await server.EchoAsync("redis")).ToString();
            }
            finally
            {
                await TestTools.StopHostedAsync(serviceProvider);
            }

            // Assert
            Assert.Equal("redis", echo);
        }
    }
}
