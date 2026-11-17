namespace MyLab.Redis.Scripting;

/// <summary>
/// Lua script commands for one Redis database.
/// </summary>
public interface IRedisScriptTools
{
    /// <summary>
    /// Kills the currently executing Lua script, assuming no write operation was yet performed by the script.
    /// </summary>
    Task KillCurrentAsync();

    /// <summary>
    /// Load a script into the scripts cache, without executing it
    /// </summary>
    /// <param name="script">script text</param>
    /// <returns>SHA1 digest of the script added into the script cache</returns>
    Task<string> LoadAsync(string script);

    /// <summary>
    /// Returns information about the existence of the scripts in the script cache.
    /// </summary>
    /// <returns>
    /// Array reply The command returns an array of bool that correspond to the specified SHA1 digest arguments.
    /// </returns>
    Task<bool[]> ExistsAsync(params string[] sha1Arr);

    /// <summary>
    /// Returns information about the existence of the scripts in the script cache.
    /// </summary>
    /// <returns>
    /// Array reply The command returns an array of bool that correspond to the specified SHA1 digest arguments.
    /// </returns>
    Task<bool> ExistsAsync(string sha1);

    /// <summary>
    /// Flush the Lua scripts cache
    /// </summary>
    Task FlushCacheAsync();

    /// <summary>
    /// Creates evaluation builder for inline script
    /// </summary>
    RedisScriptEvaluationBuilder Inline(string script);

    /// <summary>
    /// Creates evaluation builder for script by SHA1
    /// </summary>
    RedisScriptEvaluationBuilder BySha(string sha1);
}