using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.List
{
    /// <summary>
    /// RPOP redis command
    /// </summary>
    public class ListRightPopRedisCmd : FuncRedisCommand<string, BulkStringRedisValue>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ListRightPopRedisCmd"/>
        /// </summary>
        public ListRightPopRedisCmd(string key) : base("RPOP", key)
        {
            
        }

        /// <inheritdoc />
        protected override string ConvertResponse(BulkStringRedisValue responseValue)
        {
            return responseValue.Value;
        }
    }
}
