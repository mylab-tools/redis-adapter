using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Strings
{
    /// <summary>
    /// INCR command
    /// </summary>
    public class IncrRedisCmd : FuncRedisCommand<int, IntegerRedisValue>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IncrRedisCmd"/>
        /// </summary>
        public IncrRedisCmd(string key) : base("INCR", key)
        {
            
        }

        /// <inheritdoc />
        protected override int ConvertResponse(IntegerRedisValue responseValue)
        {
            return responseValue.Value;
        }
    }
}
