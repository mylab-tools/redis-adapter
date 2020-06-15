using System.Linq;
using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.List
{
    /// <summary>
    /// LRANGE redis command
    /// </summary>
    public class ListRangeRedisCmd : FuncRedisCommand<string[], ArrayRedisValue>
    {
        /// <summary>
        /// Start index
        /// </summary>
        public int StartIndex { get; set; } = 0;
        /// <summary>
        /// End index
        /// </summary>
        public int EndIndex { get; set; } = -1;
        
        /// <inheritdoc />
        public ListRangeRedisCmd(string key) : base("LRANGE", key)
        {
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            parameters.Add(StartIndex.ToString());
            parameters.Add(EndIndex.ToString());
        }
        
        /// <inheritdoc />
        protected override string[] ConvertResponse(ArrayRedisValue responseValue)
        {
            return responseValue.Items.Select(itm => ((BulkStringRedisValue) itm).Value).ToArray();
        }
    }
}