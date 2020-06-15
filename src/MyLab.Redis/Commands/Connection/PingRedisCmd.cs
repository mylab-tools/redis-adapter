using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Connection
{
    
    /// <summary>
    /// PING command
    /// </summary>
    public class PingRedisCmd : FuncRedisCommand<bool, StringRedisValue>
    {
        /// <inheritdoc />
        public PingRedisCmd() : base("PING")
        {
        }

        /// <inheritdoc />
        protected override bool ConvertResponse(StringRedisValue responseValue)
        {
            return responseValue.Value == "PONG";
        }
    }
}
