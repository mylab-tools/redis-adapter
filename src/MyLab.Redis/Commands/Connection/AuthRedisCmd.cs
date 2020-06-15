using System.Collections.Generic;
using MyLab.Redis.CommandModel;

namespace MyLab.Redis.Commands.Connection
{
    /// <summary>
    /// AUTH redis command
    /// </summary>
    public class AuthRedisCmd : ActionRedisCommand
    {
        /// <summary>
        /// Gets connection password
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="AuthRedisCmd"/>
        /// </summary>
        public AuthRedisCmd(string password) : base("AUTH")
        {
            Password = password;
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            parameters.Add(Password);
        }
    }
}
