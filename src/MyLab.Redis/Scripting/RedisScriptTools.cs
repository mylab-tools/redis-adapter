using StackExchange.Redis;

namespace MyLab.Redis.Scripting;

public class RedisScriptTools(IDatabaseAsync redisDb) : IRedisScriptTools
{
    public Task KillCurrentAsync() => ExecScriptAsync("KILL");

    public async Task<string> LoadAsync(string script)
    {
        var res = await ExecScriptAsync("LOAD", script);
        return res.ToString();
    }

    public async Task<bool[]> ExistsAsync(params string[] sha1Arr)
    {
        string[] executeArgsStr = ["EXISTS", .. sha1Arr];

        var res = await ExecScriptAsync(executeArgsStr);

        if (res.IsNull)
        {
            throw new InvalidOperationException("SCRIPT EXISTS returns NULL");
        }

        return (bool[])res!;
    }

    public async Task<bool> ExistsAsync(string sha1)
    {
        var res = await ExistsAsync([sha1]);
        return res[0];
    }

    public async Task FlushCacheAsync() => await ExecScriptAsync("FLUSH", "SYNC");

    public RedisScriptEvaluationBuilder Inline(string script)
    {
        var evaluator = new InlineRedisScriptEvaluator(redisDb, script);
        return new RedisScriptEvaluationBuilder(evaluator);
    }

    public RedisScriptEvaluationBuilder BySha(string sha1)
    {
        var evaluator = new ShaRedisScriptEvaluator(redisDb, sha1);
        return new RedisScriptEvaluationBuilder(evaluator);
    }

    Task<RedisResult> ExecScriptAsync(params string[] args)
    {
        List<object> executeArgs = [.. args];
        return redisDb.ExecuteAsync("SCRIPT", executeArgs);
    }
}