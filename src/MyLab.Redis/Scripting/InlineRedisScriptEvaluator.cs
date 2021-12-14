using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.Scripting
{
    class InlineRedisScriptEvaluator : IRedisScriptEvaluator
    {
        private readonly RedisDbProvider _dbProvider;
        private readonly string _script;

        public InlineRedisScriptEvaluator(RedisDbProvider dbProvider, string script)
        {
            _dbProvider = dbProvider;
            _script = script;
        }

        public Task<RedisResult> EvaluateScriptAsync(RedisKey[] keys, RedisValue[] args)
        {
            return _dbProvider.Provide().ScriptEvaluateAsync(_script, keys, args);
        }
    }
}