using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MyLab.Redis.Services;

namespace MyLab.Redis.HealthCheck
{
    class RedisConnectionHealthCheck : IHealthCheck
    {
        private readonly IRedisConnectionProvider _connectionProvider;
        private readonly RedisOptions _opt;

        public RedisConnectionHealthCheck(IRedisConnectionProvider connectionProvider, IOptions<RedisOptions> options)
        {
            _connectionProvider = connectionProvider;
            _opt = options.Value;
        }

        /// <inheritdoc/>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            bool hasConnection;
            Exception exception = null;

            try
            {
                hasConnection = _connectionProvider.Provide() != null;
            }
            catch (RedisNotConnectedException)
            {
                hasConnection = false;
            }
            catch (Exception e)
            {
                hasConnection = false;
                exception = e;
            }

            string description = hasConnection
                ? "Connection established"
                : "No connection established";
            
            var check = new HealthCheckResult(
                hasConnection
                    ? HealthStatus.Healthy
                    : HealthStatus.Unhealthy,
                description,
                exception,
                new Dictionary<string, object>
                {
                    {"connection-string", _opt.ConnectionString}
                }
            );

            return Task.FromResult(check);
        }
    }
}
