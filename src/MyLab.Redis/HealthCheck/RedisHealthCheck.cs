using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MyLab.Redis.Abstractions;
using MyLab.Redis.Options;
using StackExchange.Redis;

namespace MyLab.Redis.HealthCheck;

class RedisHealthCheck(IRedisService redis, IOptions<RedisOptions> options) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        IRedisServer[]? servers = null;

        try
        {
            servers = redis.GetAllServers();
        }
        catch (RedisException)
        {
        }

        bool[]? pingResults = null;

        if (servers != null)
        {
            pingResults = await PingServersAsync(servers);
        }

        if (pingResults == null || pingResults.Length == 0 || pingResults.All(r => !r))
        {
            return options.Value.Optional 
                ? HealthCheckResult.Healthy("Redis connection is not established but is optional") 
                : HealthCheckResult.Unhealthy("Redis connection is not established");
        }

        return pingResults.All(r => r) ? 
            HealthCheckResult.Healthy("Redis connection established") : 
            HealthCheckResult.Degraded($"Redis connection degradated: {pingResults.Count(r => r)}/{pingResults.Length}");
    }

    async Task<bool[]> PingServersAsync(IRedisServer[] servers)
    {
        var pingTasks = servers.Select(async s =>
        {
            try
            {
                await s.PingAsync();
                return true;
            }
            catch (RedisException)
            {
                return false;
            }
        });

        return await Task.WhenAll(pingTasks);
    }
}