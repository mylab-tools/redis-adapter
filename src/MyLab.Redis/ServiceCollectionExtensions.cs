using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyLab.Redis.Abstractions;
using MyLab.Redis.Connection;
using MyLab.Redis.Core;
using MyLab.Redis.Options;

#pragma warning disable IDE0130
namespace MyLab.Redis;
#pragma warning restore IDE0130

/// <summary>
/// Registers Redis services in dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Redis connection, <see cref="IRedisService"/>, and the hosted service that connects on startup.
    /// </summary>
    /// <param name="services">Service collection.</param>
    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        return services.AddSingleton<RedisService>()
            .AddSingleton<IRedisConnectionProvider>(p => p.GetRequiredService<RedisService>())
            .AddSingleton<IRedisConnectionStorageCore>(p => p.GetRequiredService<RedisService>())
            .AddSingleton<IRedisService>(p => p.GetRequiredService<RedisService>())
            .AddHostedService<ConnectionBackgroundService>();
    }

    /// <summary>
    /// Binds <see cref="RedisOptions"/> from configuration section <paramref name="sectionName"/>.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="config">Application configuration.</param>
    /// <param name="sectionName">Configuration section name.</param>
    public static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration config, string sectionName = RedisOptions.SectionName)
    {
        return services.Configure<RedisOptions>(config.GetSection(sectionName));
    }

    /// <summary>
    /// Binds <see cref="RedisOptions"/> from <paramref name="configureOptions"/>.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configureOptions">Options assignment.</param>
    public static IServiceCollection ConfigureRedis(this IServiceCollection services, Action<RedisOptions> configureOptions)
    {
        return services.Configure(configureOptions);
    }
}