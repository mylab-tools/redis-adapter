using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Strings
{
    /// <summary>
    /// GET command
    /// </summary>
    public class GetRedisCmd : FuncRedisCommand<string, BulkStringRedisValue>
    {
        /// <inheritdoc />
        public GetRedisCmd(string key) : base("GET", key)
        {
        }

        /// <inheritdoc />
        protected override string ConvertResponse(BulkStringRedisValue responseValue)
        {
            return responseValue.Value;
        }
    }
}
