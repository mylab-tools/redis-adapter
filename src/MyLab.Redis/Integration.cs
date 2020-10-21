using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyLab.Redis
{
    /// <summary>
    /// Integration
    /// </summary>
    public static class RedisIntegration
    {
        public static IServiceCollection AddRedisUsage(this IServiceCollection services, IConfiguration configuration, string sectionName = "Redis")
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (sectionName == null) throw new ArgumentNullException(nameof(sectionName));

            return services.Configure<RedisOptions>(configuration.GetSection(sectionName));
        }
    }
}
