using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0130
namespace MyLab.Redis.HealthCheck
#pragma warning restore IDE0130
{
    /// <summary>
    /// Registers the Redis health check.
    /// </summary>
    public static class HealthChecksBuilderExtensions
    {
        /// <summary>
        /// Adds the Redis health check named <c>redis</c>.
        /// </summary>
        /// <param name="builder">Health-check builder.</param>
        public static IHealthChecksBuilder AddRedis(this IHealthChecksBuilder builder)
        {
            return builder.AddCheck<RedisHealthCheck>("redis");
        }
    }
}
