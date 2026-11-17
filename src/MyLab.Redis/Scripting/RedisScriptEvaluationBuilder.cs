using MyLab.Redis.Keys;
using StackExchange.Redis;

namespace MyLab.Redis.Scripting;

/// <summary>
/// Collects keys and arguments and evaluates a Lua script.
/// </summary>
public class RedisScriptEvaluationBuilder
{
    private readonly IRedisScriptEvaluator _scriptEvaluator;

    private readonly RedisKey[]? _keys;

    private readonly RedisValue[]? _args;

    /// <summary>
    /// Initializes a builder for <paramref name="scriptEvaluator"/>.
    /// </summary>
    /// <param name="scriptEvaluator">Evaluator that runs the script.</param>
    public RedisScriptEvaluationBuilder(IRedisScriptEvaluator scriptEvaluator)
    {
        _scriptEvaluator = scriptEvaluator;
    }

    RedisScriptEvaluationBuilder(IRedisScriptEvaluator scriptEvaluator, RedisKey[]? keys, RedisValue[]? args)
    {
        _scriptEvaluator = scriptEvaluator;
        _keys = keys;
        _args = args;
    }

    /// <summary>
    /// Adds keys for script evaluation
    /// </summary>
    public RedisScriptEvaluationBuilder WithKeys(params string[] keys)
    {
        var newKeys = new List<RedisKey>();

        if (_keys != null)
            newKeys.AddRange(_keys);

        newKeys.AddRange(keys.Select(k => (RedisKey)k));

        return new RedisScriptEvaluationBuilder(_scriptEvaluator, [.. newKeys], _args);
    }

    /// <summary>
    /// Adds key for script evaluation
    /// </summary>
    public RedisScriptEvaluationBuilder WithKey(RedisKeyBase key)
    {
        return WithKeys(key.Key);
    }

    /// <summary>
    /// Adds args for script evaluation
    /// </summary>
    public RedisScriptEvaluationBuilder WithArgs(params RedisValue[] args)
    {
        var newArgs = new List<RedisValue>();

        if (_args != null)
            newArgs.AddRange(_args);

        newArgs.AddRange(args);

        return new RedisScriptEvaluationBuilder(_scriptEvaluator, _keys, [.. newArgs]);
    }

    /// <summary>
    /// Evaluates script
    /// </summary>
    public Task<RedisResult> EvaluateAsync()
    {
        return _scriptEvaluator.EvaluateAsync(_keys ?? [], _args ?? []);
    }
}