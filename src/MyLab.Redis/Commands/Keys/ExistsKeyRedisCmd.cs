using System.Collections.Generic;
using MyLab.Redis.CommandModel;

namespace MyLab.Redis.Commands.Keys
{
    /// <summary>
    /// EXISTS command
    /// </summary>
    public class ExistsKeyRedisCmd : MultiKeyRedisCmd
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ExistsKeyRedisCmd"/>
        /// </summary>
        public ExistsKeyRedisCmd(string key, params string[] keys) : base("EXISTS", key, keys)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ExistsKeyRedisCmd"/>
        /// </summary>
        public ExistsKeyRedisCmd(IEnumerable<string> initialKeys) : base("EXISTS", initialKeys)
        {
        }
    }
}