using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.Scripting
{
    /// <summary>
    /// Defines case relative redis script evaluator
    /// </summary>
    public interface IRedisScriptEvaluator
    {
        /// <summary>
        /// Evaluates script
        /// </summary>
        Task<RedisResult> EvaluateScriptAsync(RedisKey[] keys, RedisValue[] args);
    }
}