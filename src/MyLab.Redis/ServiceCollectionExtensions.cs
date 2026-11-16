using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyLab.Redis.Abstractions;
using MyLab.Redis.Connection;
using MyLab.Redis.Core;
using MyLab.Redis.Options;

#pragma warning disable IDE0130
namespace MyLab.Redis;
#pragma warning restore IDE0130

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        return services.AddSingleton<RedisService>()
            .AddSingleton<IRedisConnectionProvider>(p => p.GetRequiredService<RedisService>())
            .AddSingleton<IRedisConnectionStorageCore>(p => p.GetRequiredService<RedisService>())
            .AddSingleton<IRedisService>(p => p.GetRequiredService<RedisService>())
            .AddHostedService<ConnectionBackgroundService>();
    }

    public static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration config, string sectionName = RedisOptions.SectionName)
    {
        return services.Configure<RedisOptions>(config.GetSection(sectionName));
    }

    public static IServiceCollection ConfigureRedis(this IServiceCollection services, Action<RedisOptions> configureOptions)
    {
        return services.Configure(configureOptions);
    }
}