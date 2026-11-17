using StackExchange.Redis;

namespace MyLab.Redis.Scripting;

class InlineRedisScriptEvaluator(IDatabaseAsync redisDb, string script) : IRedisScriptEvaluator
{
    public Task<RedisResult> EvaluateAsync(RedisKey[] keys, RedisValue[] args)
        => redisDb.ScriptEvaluateAsync(script, keys, args);
}