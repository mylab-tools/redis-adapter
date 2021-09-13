using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyLab.Redis.Services;

namespace MyLab.Redis
{
    /// <summary>
    /// Contains extensions for <see cref="IServiceCollection"/>
    /// </summary>
    public static class RedisIntegration
    {
        /// <summary>
        /// Adds Redis services
        /// </summary>
        public static IServiceCollection AddRedis(this IServiceCollection services, RedisConnectionStrategy redisConnectionStrategy)
        {
            services.AddSingleton<IRedisService, RedisService>();

            switch (redisConnectionStrategy)
            {
                case RedisConnectionStrategy.Lazy:
                    services.AddSingleton<IRedisConnectionProvider, LazyRedisConnectionProvider>();
                    break;
                case RedisConnectionStrategy.Background:
                    services
                        .AddSingleton<IRedisConnectionProvider, BackgroundRedisConnectionProvider>()
                        .AddSingleton<IRedisConnectionManager, RedisBackgroundConnectionManager>()
                        .AddHostedService<RedisConnectionStarter>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(redisConnectionStrategy), "Redis connection strategy must be defined");
            }

            return services;
        }

        /// <summary>
        /// Configures Redis options with configuration section
        /// </summary>
        public static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration configuration, string sectionName = "Redis")
        {
            return services.Configure<RedisOptions>(configuration.GetSection(sectionName));
        }

        /// <summary>
        /// Configures Redis options with configuration action
        /// </summary>
        public static IServiceCollection ConfigureRedis(this IServiceCollection services, Action<RedisOptions> configureOptions)
        {
            return services.Configure(configureOptions);
        }
    }
}
