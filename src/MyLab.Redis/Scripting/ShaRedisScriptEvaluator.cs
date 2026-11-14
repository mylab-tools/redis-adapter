using StackExchange.Redis;

namespace MyLab.Redis.Scripting;

class ShaRedisScriptEvaluator(IDatabaseAsync redisDb, string scriptSha) : IRedisScriptEvaluator
{
    private readonly byte[] _scriptSha= Enumerable.Range(0, scriptSha.Length)
        .Where(x => x % 2 == 0)
        .Select(x => Convert.ToByte(scriptSha.Substring(x, 2), 16))
        .ToArray();

    public Task<RedisResult> EvaluateAsync(RedisKey[] keys, RedisValue[] args)
        => redisDb.ScriptEvaluateAsync(_scriptSha, keys, args);
}