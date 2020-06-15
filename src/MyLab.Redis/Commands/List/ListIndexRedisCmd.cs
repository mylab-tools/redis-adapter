using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.List
{
    /// <summary>
    /// LINDEX redis command
    /// </summary>
    public class ListIndexRedisCmd : FuncRedisCommand<string, BulkStringRedisValue>
    {
        /// <summary>
        /// GEts or sets item index
        /// </summary>
        public int Index { get; set; } = -1;

        /// <summary>
        /// Initializes a new instance of <see cref="ListIndexRedisCmd"/>
        /// </summary>
        public ListIndexRedisCmd(string key) : base("LINDEX", key)
        {
            
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            parameters.Add(Index.ToString());
        }

        /// <inheritdoc />
        protected override string ConvertResponse(BulkStringRedisValue responseValue)
        {
            return responseValue.Value;
        }
    }
}
