using StackExchange.Redis;

namespace MyLab.Redis.Scripting;

/// <summary>
/// Lua script commands for one Redis database.
/// </summary>
public class RedisScriptTools(IDatabaseAsync redisDb) : IRedisScriptTools
{
    /// <inheritdoc />
    public Task KillCurrentAsync() => ExecScriptAsync("KILL");

    /// <inheritdoc />
    public async Task<string> LoadAsync(string script)
    {
        var res = await ExecScriptAsync("LOAD", script);
        return res.ToString();
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string sha1)
    {
        var res = await ExistsAsync([sha1]);
        return res[0];
    }

    /// <inheritdoc />
    public async Task FlushCacheAsync() => await ExecScriptAsync("FLUSH", "SYNC");

    /// <inheritdoc />
    public RedisScriptEvaluationBuilder Inline(string script)
    {
        var evaluator = new InlineRedisScriptEvaluator(redisDb, script);
        return new RedisScriptEvaluationBuilder(evaluator);
    }

    /// <inheritdoc />
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