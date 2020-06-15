using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.List
{
    /// <summary>
    /// LLEN redis command
    /// </summary>
    public class ListLengthRedisCmd : FuncRedisCommand<int, IntegerRedisValue>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ListLengthRedisCmd"/>
        /// </summary>
        public ListLengthRedisCmd(string key) : base("LLEN", key)
        {
        }

        /// <inheritdoc />
        protected override int ConvertResponse(IntegerRedisValue responseValue)
        {
            return responseValue.Value;
        }
    }
}
