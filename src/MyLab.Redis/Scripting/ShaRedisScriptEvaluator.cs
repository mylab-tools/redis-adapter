using System;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.Scripting
{
    class ShaRedisScriptEvaluator : IRedisScriptEvaluator
    {
        private readonly RedisDbProvider _dbProvider;
        private readonly byte[] _scriptSha;

        public ShaRedisScriptEvaluator(RedisDbProvider dbProvider, string scriptSha)
        {
            _dbProvider = dbProvider;
            _scriptSha = Enumerable.Range(0, scriptSha.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(scriptSha.Substring(x, 2), 16))
                .ToArray();
        }

        public Task<RedisResult> EvaluateScriptAsync(RedisKey[] keys, RedisValue[] args)
        {
            return _dbProvider.Provide().ScriptEvaluateAsync(_scriptSha, keys, args);
        }
    }
}