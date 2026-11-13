using StackExchange.Redis;

namespace MyLab.Redis.Abstractions
{
    internal interface IRedisConnectionStorageCore
    {
        void SaveConnection(IConnectionMultiplexer connectionMultiplexer);
    }
}
