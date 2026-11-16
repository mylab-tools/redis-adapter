using StackExchange.Redis;

namespace MyLab.Redis.Scripting;

/// <summary>
/// Evaluates a Lua script.
/// </summary>
public interface IRedisScriptEvaluator
{
    /// <summary>
    /// Evaluates the script with <paramref name="keys"/> and <paramref name="args"/>.
    /// </summary>
    /// <param name="keys">Keys available to the script as <c>KEYS</c>.</param>
    /// <param name="args">Arguments available to the script as <c>ARGV</c>.</param>
    /// <returns>The script result.</returns>
    Task<RedisResult> EvaluateAsync(RedisKey[] keys, RedisValue[] args);
}