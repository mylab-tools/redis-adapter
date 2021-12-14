using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.Scripting
{
    /// <summary>
    /// Provides script tools
    /// </summary>
    public class RedisScriptTools
    {
        private readonly RedisDbProvider _dbProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="RedisScriptTools"/>
        /// </summary>
        public RedisScriptTools(RedisDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        /// <summary>
        /// Kills the currently executing Lua script, assuming no write operation was yet performed by the script.
        /// </summary>
        public async Task KillCurrentAsync()
        {
            await _dbProvider.Provide().ExecuteAsync("SCRIPT", "KILL");
        }

        /// <summary>
        /// Load a script into the scripts cache, without executing it
        /// </summary>
        /// <param name="script">script text</param>
        /// <returns>SHA1 digest of the script added into the script cache</returns>
        public async Task<string> LoadAsync(string script)
        {
            var res = await _dbProvider.Provide().ExecuteAsync("SCRIPT", "LOAD", script);

            return res.ToString();
        }

        /// <summary>
        /// Returns information about the existence of the scripts in the script cache.
        /// </summary>
        /// <returns>
        /// Array reply The command returns an array of bool that correspond to the specified SHA1 digest arguments.
        /// </returns>
        public async Task<bool[]> ExistsAsync(params string[] sha1Arr)
        {
            var args = new List<object>
            {
                "EXISTS"
            };

            if (sha1Arr != null)
            {
                args.AddRange(sha1Arr);
            }

            var res = await _dbProvider.Provide().ExecuteAsync("SCRIPT", args);

            return (bool[])res;
        }

        /// <summary>
        /// Returns information about the existence of the scripts in the script cache.
        /// </summary>
        /// <returns>
        /// Array reply The command returns an array of bool that correspond to the specified SHA1 digest arguments.
        /// </returns>
        public async Task<bool> ExistsAsync(string sha1)
        {
            var res = await ExistsAsync(new [] {sha1});
            return res[0];
        }

        /// <summary>
        /// Flush the Lua scripts cache
        /// </summary>
        public async Task FlushCacheAsync()
        {
            await _dbProvider.Provide().ExecuteAsync("SCRIPT", "FLUSH", "SYNC");
        }

        /// <summary>
        /// Creates evaluation builder for inline script
        /// </summary>
        public RedisScriptEvaluationBuilder Inline(string script)
        {
            var evaluator = new InlineRedisScriptEvaluator(_dbProvider, script);

            return new RedisScriptEvaluationBuilder(evaluator);
        }

        /// <summary>
        /// Creates evaluation builder for script by SHA1
        /// </summary>
        public RedisScriptEvaluationBuilder BySha(string sha1)
        {
            var evaluator = new ShaRedisScriptEvaluator(_dbProvider, sha1);

            return new RedisScriptEvaluationBuilder(evaluator);
        }
    }
}