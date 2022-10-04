using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyLab.Redis.Connection;
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
        public static IServiceCollection AddRedis(this IServiceCollection services, IRedisConnectionPolicy connectionPolicy)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (connectionPolicy == null) throw new ArgumentNullException(nameof(connectionPolicy));

            services.AddSingleton<IRedisService, RedisService>();

            connectionPolicy.RegisterDependencies(services);

            return services;
        }

        /// <summary>
        /// Adds Redis services
        /// </summary>
        [Obsolete("Use AddRedis(this IServiceCollection services, IRedisConnectionPolicy connectionPolicy) instead", true)]
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
                        .AddSingleton<IBackgroundRedisConnectionManager, BackgroundRedisConnectionManager>()
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
