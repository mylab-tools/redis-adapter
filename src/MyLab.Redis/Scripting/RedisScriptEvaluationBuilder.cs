using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MyLab.Redis.ObjectModel;
using StackExchange.Redis;

namespace MyLab.Redis.Scripting
{
    /// <summary>
    /// Collect script evaluation parameters and evaluate script
    /// </summary>
    public class RedisScriptEvaluationBuilder
    {
        private readonly IRedisScriptEvaluator _scriptEvaluator;

        private readonly RedisKey[] _keys;

        private readonly RedisValue[] _args;

        /// <summary>
        /// Initializes a new instance of <see cref="RedisScriptEvaluationBuilder"/>
        /// </summary>
        public RedisScriptEvaluationBuilder(IRedisScriptEvaluator scriptEvaluator)
        {
            _scriptEvaluator = scriptEvaluator;
        }

        RedisScriptEvaluationBuilder(IRedisScriptEvaluator scriptEvaluator, RedisKey[] keys, RedisValue[] args)
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

            if(_keys != null)
                newKeys.AddRange(_keys);
            
            if (keys != null)
                newKeys.AddRange(keys.Select(k => (RedisKey)k));

            return new RedisScriptEvaluationBuilder(_scriptEvaluator, newKeys.ToArray(), _args);
        }

        /// <summary>
        /// Adds key for script evaluation
        /// </summary>
        public RedisScriptEvaluationBuilder WithKey(RedisKeyBase key)
        {
            return WithKeys(key.KeyName);
        }

        /// <summary>
        /// Adds args for script evaluation
        /// </summary>
        public RedisScriptEvaluationBuilder WithArgs(params RedisValue[] args)
        {
            var newArgs = new List<RedisValue>();

            if(_args != null)
                newArgs.AddRange(_args);

            if(args != null)
                newArgs.AddRange(args);

            return new RedisScriptEvaluationBuilder(_scriptEvaluator, _keys, newArgs.ToArray());
        }

        /// <summary>
        /// Evaluates script
        /// </summary>
        public Task<RedisResult> EvaluateAsync()
        {
            return _scriptEvaluator.EvaluateScriptAsync(_keys, _args);
        }
    }
}