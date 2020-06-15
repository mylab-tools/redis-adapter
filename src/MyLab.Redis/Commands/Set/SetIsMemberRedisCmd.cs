using System;
using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Set
{
    /// <summary>
    /// SISMEMBER command
    /// </summary>
    public class SetIsMemberRedisCmd : FuncRedisCommand<bool, IntegerRedisValue>
    {
        /// <summary>
        /// Gets or sets search value 
        /// </summary>
        public string Member { get; }

        /// <inheritdoc />
        public SetIsMemberRedisCmd(string key, string member) : base("SISMEMBER", key)
        {
            Member = member ?? throw new ArgumentNullException(nameof(member));
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            parameters.Add(Member);
        }

        /// <inheritdoc />
        protected override bool ConvertResponse(IntegerRedisValue responseValue)
        {
            return responseValue.Value == 1;
        }
    }
}
