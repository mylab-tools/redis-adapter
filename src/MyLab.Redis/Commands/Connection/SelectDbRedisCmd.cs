using System.Collections.Generic;
using MyLab.Redis.CommandModel;

namespace MyLab.Redis.Commands.Connection
{
    /// <summary>
    /// SELECT redis command
    /// </summary>
    public class SelectDbRedisCmd : ActionRedisCommand
    {
        /// <summary>
        /// Gets data base index
        /// </summary>
        public int DbIndex { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="SelectDbRedisCmd"/>
        /// </summary>
        public SelectDbRedisCmd(int dbIndex) : base("SELECT")
        {
            DbIndex = dbIndex;
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            parameters.Add(DbIndex.ToString());
        }
    }
}
