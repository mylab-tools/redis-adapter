using System;
using System.Collections.Generic;
using System.Linq;
using MyLab.Redis.CommandModel;

namespace MyLab.Redis.Commands.Hash
{
    /// <summary>
    /// HMSET command 
    /// </summary>
    public class HashMultiSetRedisCmd : ActionRedisCommand
    {
        /// <summary>
        /// Gets hash fields
        /// </summary>
        public IDictionary<string, string> Fields{ get; }

        /// <summary>
        /// Initializes a new instance of <see cref="HashMultiSetRedisCmd"/>
        /// </summary>
        public HashMultiSetRedisCmd(string key)
            :this(key, new Dictionary<string, string>())
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HashMultiSetRedisCmd"/>
        /// </summary>
        public HashMultiSetRedisCmd(string key, IDictionary<string, string> fields)
            : base("HMSET", key)
        {
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters paramters)
        {
            if (Fields.Count == 0)
                throw new InvalidOperationException("Field list is empty");

            paramters.AddRange(Fields.SelectMany(f => 
                Enumerable.Repeat(f.Key, 1).Union(
                Enumerable.Repeat(f.Value, 1))));
        }
    }
}
