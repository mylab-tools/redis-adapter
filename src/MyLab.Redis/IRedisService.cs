using MyLab.Redis.ObjectModel;

namespace MyLab.Redis
{
    /// <summary>
    /// Provides Redis interaction features
    /// </summary>
    public interface IRedisService
    {
        StringRedisKey StringKey(string key);
        Int64RedisKey Int64Key(string key);
        DoubleRedisKey DoubleKey(string key);
    }
}
