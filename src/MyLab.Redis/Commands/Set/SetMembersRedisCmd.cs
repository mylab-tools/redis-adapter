using System.Linq;
using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Set
{
    /// <summary>
    /// SMEMBERS command
    /// </summary>
    public class SetMembersRedisCmd : FuncRedisCommand<string[], ArrayRedisValue>
    {
        /// <inheritdoc />
        public SetMembersRedisCmd(string key) : base("SMEMBERS", key)
        {
        }

        /// <inheritdoc />
        protected override string[] ConvertResponse(ArrayRedisValue responseValue)
        {
            return responseValue.Items.Select(itm => ((BulkStringRedisValue) itm).Value).ToArray();
        }
    }
}
