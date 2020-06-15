using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.List
{
    /// <summary>
    /// LPOP redis command
    /// </summary>
    public class ListLeftPopRedisCmd : FuncRedisCommand<string, BulkStringRedisValue>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ListLeftPopRedisCmd"/>
        /// </summary>
        public ListLeftPopRedisCmd(string key) : base("LPOP", key)
        {
            
        }

        /// <inheritdoc />
        protected override string ConvertResponse(BulkStringRedisValue responseValue)
        {
            return responseValue.Value;
        }
    }
}
