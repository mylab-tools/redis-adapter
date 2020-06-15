using System;
using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Keys
{
    /// <summary>
    /// PEXPIRE redis command
    /// </summary>
    public class ExpireRedisCommand : FuncRedisCommand<bool, IntegerRedisValue>
    {
        /// <summary>
        /// Gets life time in <see cref="TimeSpan"/>
        /// </summary>
        public TimeSpan TimeSpan { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ExpireRedisCommand"/>
        /// </summary>
        public ExpireRedisCommand(string key, TimeSpan timeSpan) : base("PEXPIRE", key)
        {
            TimeSpan = timeSpan;
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            base.GetParameters(parameters);
            parameters.Add(TimeSpan.TotalMilliseconds.ToString("F0"));
        }

        /// <inheritdoc />
        protected override bool ConvertResponse(IntegerRedisValue responseValue)
        {
            return responseValue.Value == 1;
        }
    }
}
