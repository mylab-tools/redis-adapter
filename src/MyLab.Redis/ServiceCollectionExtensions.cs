using Microsoft.Extensions.DependencyInjection;
using MyLab.Redis.Abstractions;
using MyLab.Redis.Connection;

namespace MyLab.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        return services.AddSingleton<RedisConnectionProvider>()
            .AddSingleton<IRedisConnectionProvider>(p => p.GetRequiredService<RedisConnectionProvider>())
            .AddSingleton<IRedisConnectionStorageCore>(p => p.GetRequiredService<RedisConnectionProvider>())
            .AddHostedService<ConnectionBackgroundService>();
    }
}