using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Strings
{
    /// <summary>
    /// DECR command
    /// </summary>
    public class DecrRedisCmd : FuncRedisCommand<int, IntegerRedisValue>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IncrRedisCmd"/>
        /// </summary>
        public DecrRedisCmd(string key) : base("DECR", key)
        {
            
        }

        /// <inheritdoc />
        protected override int ConvertResponse(IntegerRedisValue responseValue)
        {
            return responseValue.Value;
        }
    }
}
