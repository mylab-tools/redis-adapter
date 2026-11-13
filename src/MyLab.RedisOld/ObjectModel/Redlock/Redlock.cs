using System;
using System.Threading.Tasks;
using MyLab.Redis.Properties;
using MyLab.Redis.Scripting;
using StackExchange.Redis;

namespace MyLab.Redis.ObjectModel
{
    /// <summary>
    /// Provides inter-process lock
    /// </summary>
    public class Redlock : IAsyncDisposable
    {
        private readonly StringRedisKey _key;
        private readonly string _id;
        private readonly RedisScriptTools _scriptTools;

        /// <summary>
        /// Gets or sets default prolongation time span
        /// </summary>
        public TimeSpan DefaultProlongSpan { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Redlock"/>
        /// </summary>
        public Redlock(StringRedisKey key, string id, RedisScriptTools scriptTools)
        {
            _key = key;
            _id = id;
            _scriptTools = scriptTools;
        }

        /// <summary>
        /// Retrieves the state from Redis and analysis an acquirement 
        /// </summary>
        /// <returns>true - if is acquired</returns>
        public async Task<bool> IsAcquiredAsync()
        {
            var keyValue = await _key.GetAsync();

            return keyValue.HasValue && keyValue == _id;
        }

        /// <summary>
        /// Prolongs a locking for default time span
        /// </summary>
        /// <returns>true - if successful</returns>
        public Task<bool> ProlongAsync()
        {
            return ProlongAsync(DefaultProlongSpan);
        }

        /// <summary>
        /// Prolongs a locking for specified time span
        /// </summary>
        /// <returns>true - if successful</returns>
        public async Task<bool> ProlongAsync(TimeSpan timeSpan)
        {
            var scriptEval = _scriptTools
                .Inline(Resources.ProlongRedlockLua)
                .WithArgs(_id)
                .WithArgs(timeSpan.TotalSeconds)
                .WithKey(_key);

            var res = await scriptEval.EvaluateAsync();

            return res.IsNull && res.Type == ResultType.Integer && (int)res == 1;
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            var scriptEval = _scriptTools
                .Inline(Resources.DeleteRedlockLua)
                .WithArgs(_id)
                .WithKey(_key);

            await scriptEval.EvaluateAsync();
        }
    }
}