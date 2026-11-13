using StackExchange.Redis;

namespace MyLab.Redis.Abstractions
{
    public interface IRedisConnectionProvider
    {
        IConnectionMultiplexer? Provide();
    }
}
