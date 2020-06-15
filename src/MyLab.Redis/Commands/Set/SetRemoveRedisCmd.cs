using System;
using System.Collections.Generic;
using MyLab.Redis.CommandModel;

namespace MyLab.Redis.Commands.Set
{
    /// <summary>
    /// SREM command
    /// </summary>
    public class SetRemoveRedisCmd : ActionRedisCommand
    {
        /// <summary>
        /// Members for remove
        /// </summary>
        public List<string> Members { get; } = new List<string>();

        /// <summary>
        /// Initializes a new instance of <see cref="SetRemoveRedisCmd"/>
        /// </summary>
        public SetRemoveRedisCmd(string key) : base("SREM", key)
        {
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            if(Members.Count == 0)
                throw new InvalidOperationException("Member list is empty");

            parameters.AddRange(Members);
        }
    }
}
