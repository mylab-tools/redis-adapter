using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Set
{
    /// <summary>
    /// SCARD command
    /// </summary>
    public class SetGetCardinalityRedisCmd : FuncRedisCommand<int, IntegerRedisValue>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SetGetCardinalityRedisCmd"/>
        /// </summary>
        public SetGetCardinalityRedisCmd(string key) : base("SCARD", key)
        {
        }

        /// <inheritdoc />
        protected override int ConvertResponse(IntegerRedisValue responseValue)
        {
            return responseValue.Value;
        }
    }
}