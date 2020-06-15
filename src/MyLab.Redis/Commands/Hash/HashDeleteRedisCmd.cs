using System;
using System.Collections.Generic;
using MyLab.Redis.CommandModel;

namespace MyLab.Redis.Commands.Hash
{
    /// <summary>
    /// HDEL command
    /// </summary>
    public class HashDeleteRedisCmd : ActionRedisCommand
    {
        /// <summary>
        /// Gets fields for delete
        /// </summary>
        public List<string> Fields { get; } = new List<string>();

        /// <summary>
        /// Initializes a new instance of <see cref="HashDeleteRedisCmd"/>
        /// </summary>
        public HashDeleteRedisCmd(string key) : base("HDEL", key)
        {
            
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            if(Fields.Count == 0)
                throw new InvalidOperationException("Field list is empty");

            parameters.AddRange(Fields);
        }
    }
}
