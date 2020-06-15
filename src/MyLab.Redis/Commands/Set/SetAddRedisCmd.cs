using System;
using System.Collections.Generic;
using MyLab.Redis.CommandModel;

namespace MyLab.Redis.Commands.Set
{
    /// <summary>
    /// SADD command
    /// </summary>
    public class SetAddRedisCmd : ActionRedisCommand
    {
        /// <summary>
        /// Gets or sets member for add
        /// </summary>
        public List<string> Members { get; } = new List<string>();

        /// <inheritdoc />
        public SetAddRedisCmd(string key) : base("SADD", key)
        {
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            if (Members.Count == 0)
                throw new InvalidOperationException("Member list is empty");
            
            parameters.AddRange(Members);
        }
    }
}
