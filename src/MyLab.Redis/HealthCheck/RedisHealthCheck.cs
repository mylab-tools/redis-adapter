using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MyLab.Redis.Abstractions;
using MyLab.Redis.Options;

namespace MyLab.Redis.HealthCheck
{
    class RedisHealthCheck(IRedisService redis, IOptions<RedisOptions> options) : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (redis.Connected)
                return Task.FromResult(HealthCheckResult.Healthy("Redis connection established"));

            if (options.Value.Optional)
                return Task.FromResult(HealthCheckResult.Healthy("Redis connection is not established but is optional"));

            return Task.FromResult(HealthCheckResult.Unhealthy("Redis connection is not established"));
        }
    }
}
