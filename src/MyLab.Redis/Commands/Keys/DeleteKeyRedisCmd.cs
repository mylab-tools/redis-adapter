using System.Collections.Generic;
using MyLab.Redis.CommandModel;

namespace MyLab.Redis.Commands.Keys
{
    /// <summary>
    /// DEL command
    /// </summary>
    public class DeleteKeyRedisCmd : MultiKeyRedisCmd
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DeleteKeyRedisCmd"/>
        /// </summary>
        public DeleteKeyRedisCmd(string key, params string[] keys) : base("DEL", key, keys)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DeleteKeyRedisCmd"/>
        /// </summary>
        public DeleteKeyRedisCmd(IEnumerable<string> initialKeys) : base("DEL", initialKeys)
        {
        }
    }
}