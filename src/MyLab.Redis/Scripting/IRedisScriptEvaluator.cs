using StackExchange.Redis;

namespace MyLab.Redis.Scripting;

public interface IRedisScriptEvaluator
{
    Task<RedisResult> EvaluateAsync(RedisKey[] keys, RedisValue[] args);
}