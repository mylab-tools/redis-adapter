using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyLab.Log.Dsl;
using MyLab.Redis.Abstractions;
using MyLab.Redis.Options;
using StackExchange.Redis;
using System.Text;

namespace MyLab.Redis.Connection
{
    class ConnectionBackgroundService
        (
            IRedisConnectionStorageCore connectionStorageCore,
            IOptions<RedisOptions> opts, 
            ILogger<ConnectionBackgroundService>? logger = null
        ) : IHostedService
    {
        readonly IDslLogger? _log = logger?.Dsl();
        private IConnectionMultiplexer? _connectionMultiplexer;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var o = opts.Value;

            if (o.ConnectionString == null)
            {
                if (!o.Optional)
                    _log?.Error("Redis connection is required but connection string is not specified").Write();
                return;
            }

            _log?.Action("Redis connecting...").Write();

            var sb = new StringBuilder();
            var txtWriter = new StringWriter(sb);

            _connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(o.ConnectionString, c =>
            {
                if (o.User != null)
                    c.User = o.User;
                if (o.Password!= null)
                    c.Password = o.Password;
            }, txtWriter);

            _connectionMultiplexer.ConnectionFailed += (_, args) => 
                _log?.Warning("Redis connection failed", args.Exception)
                    .AndFactIs("endpoint", args.EndPoint?.ToString())
                    .AndFactIs("failure-type", args.FailureType)
                    .Write();

            _connectionMultiplexer.ConnectionRestored += (_, args) =>
                _log?.Action("Redis connection restored")
                    .AndFactIs("endpoint", args.EndPoint?.ToString())
                    .Write();

            _connectionMultiplexer.ErrorMessage += (_, args) => 
                _log?.Error("Redis error")
                    .AndFactIs("redis-message", args.Message)
                    .Write();

            _connectionMultiplexer.InternalError += (_, args) =>
                _log?.Error("Redis internal error", args.Exception)
                    .AndFactIs("endpoint", args.EndPoint?.ToString())
                    .AndFactIs("origin", args.Origin)
                    .Write();

            _connectionMultiplexer.ServerMaintenanceEvent += (_, ev) =>
                _log?.Action("Re3dis maintenance")
                    .AndFactIs("message", ev.RawMessage)
                    .AndFactIs("start-time", ev.StartTimeUtc)
                    .Write();

            if (_connectionMultiplexer.IsConnected)
            {
                _log?.Action("Redis connected")
                    .AndFactIs("log", sb.ToString())
                    .Write();
            }

            connectionStorageCore.SaveConnection(_connectionMultiplexer);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var disposeTask = _connectionMultiplexer?.DisposeAsync();

            return disposeTask.HasValue ? disposeTask.Value.AsTask() : Task.CompletedTask;
        }
    }
}
