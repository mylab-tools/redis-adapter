using MyLab.Redis.CommandModel;

namespace MyLab.Redis.Commands.Connection
{
    /// <summary>
    /// Close connection
    /// </summary>
    public class QuitRedisCmd : ActionRedisCommand
    {
        /// <inheritdoc />
        public QuitRedisCmd() : base("QUIT")
        {
        }
    }
}
