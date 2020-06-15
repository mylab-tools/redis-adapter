using System;
using System.Threading.Tasks;
using MyLab.Redis.Connection;
using MyLab.Redis.Values;

namespace MyLab.Redis.CommandModel
{
    /// <summary>
    /// Represent a command without result
    /// </summary>
    public class ActionRedisCommand : RedisCommand
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ActionRedisCommand"/>
        /// </summary>
        protected ActionRedisCommand(string name) : base(name)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ActionRedisCommand"/>
        /// </summary>
        protected ActionRedisCommand(string name, string key) : base(name, key)
        {
        }

        /// <summary>
        /// Performs command
        /// </summary>
        public async Task PerformAsync(IRedisConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            
            var response = await connection.PerformCommandAsync(ToRedisArray());
            
            if (response is ErrorRedisValue err)
                throw err.ToException();
        }
    }
}