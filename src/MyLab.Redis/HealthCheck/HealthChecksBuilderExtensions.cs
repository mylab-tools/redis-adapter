using MyLab.Redis.HealthCheck;

#pragma warning disable IDE0130
namespace Microsoft.Extensions.DependencyInjection
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
