using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Connection
{
    /// <summary>
    /// ECHO command
    /// </summary>
    public class EchoRedisCmd : FuncRedisCommand<string, BulkStringRedisValue>
    {
        /// <summary>
        /// Gets echo message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="EchoRedisCmd"/>
        /// </summary>
        public EchoRedisCmd(string message) : base("ECHO")
        {
            Message = message;
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            parameters.Add(Message);
        }

        /// <inheritdoc />
        protected override string ConvertResponse(BulkStringRedisValue responseValue)
        {
            return responseValue.Value;
        }
    }
}
