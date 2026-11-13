using Microsoft.Extensions.DependencyInjection;

namespace MyLab.Redis.Connection
{
    /// <summary>
    /// Defines the connection policy to Redis
    /// </summary>
    public interface IRedisConnectionPolicy
    {
        /// <summary>
        /// Registers dependencies to available the policy
        /// </summary>
        void RegisterDependencies(IServiceCollection services);
    }

    /// <summary>
    /// A policy which defines a connection when it required
    /// </summary>
    public class LazyRedisConnectionPolicy : IRedisConnectionPolicy
    {
        /// <inheritdoc />
        public void RegisterDependencies(IServiceCollection services)
        {
            services.AddSingleton<IRedisConnectionProvider, LazyRedisConnectionProvider>();
        }
    }

    /// <summary>
    /// A policy which defines a connection at the start of app asynchronously
    /// </summary>
    public class BackgroundRedisConnectionPolicy : IRedisConnectionPolicy
    {
        /// <inheritdoc />
        public void RegisterDependencies(IServiceCollection services)
        {
            services
                .AddSingleton<IRedisConnectionProvider, BackgroundRedisConnectionProvider>()
                .AddSingleton<IBackgroundRedisConnectionManager, BackgroundRedisConnectionManager>()
                .AddHostedService<RedisConnectionStarter>();
        }
    }
}
