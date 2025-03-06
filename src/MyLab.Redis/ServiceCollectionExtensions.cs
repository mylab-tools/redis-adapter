using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MyLab.Redis.Connection;
using MyLab.Redis.Options;
using MyLab.Redis.Services;

namespace MyLab.Redis
{
    /// <summary>
    /// Contains extensions for <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Redis services
        /// </summary>
        public static IServiceCollection AddRedis(this IServiceCollection services, IRedisConnectionPolicy connectionPolicy)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (connectionPolicy == null) throw new ArgumentNullException(nameof(connectionPolicy));

            services.AddSingleton<IRedisService, RedisService>()
                .TryAddEnumerable(
                    ServiceDescriptor.Singleton
                        <IValidateOptions<RedisOptions>, RedisOptionsValidator>());

            connectionPolicy.RegisterDependencies(services);

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
