using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0130
namespace MyLab.Redis.HealthCheck
#pragma warning restore IDE0130
{
    public static class HealthChecksBuilderExtensions
    {
        public static IHealthChecksBuilder AddRedis(this IHealthChecksBuilder builder)
        {
            return builder.AddCheck<RedisHealthCheck>("redis");
        }
    }
}
