using System;
using System.Linq;
using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Keys
{
    /// <summary>
    /// SCAN redis command
    /// </summary>
    public class ScanRedisCmd : FuncRedisCommand<ScanResult<string>, ArrayRedisValue>, IRedisScanCmd<string>
    {
        /// <summary>
        /// Gets cursor for scan
        /// </summary>
        public int Cursor { get; }

        /// <summary>
        /// Gets or sets the amount of work that should be done at every call in order to retrieve elements from the collection.
        /// 10 by default
        /// </summary>
        public int? Count { get; set; } = 10;

        /// <summary>
        /// Gets or sets key pattern
        /// </summary>
        public string Pattern { get; set; }

        /// <inheritdoc />
        public ScanRedisCmd(int cursor) : base("SCAN")
        {
            Cursor = cursor;
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            parameters.Add(Cursor.ToString());
            parameters.AddOptional("COUNT", Count?.ToString());
            parameters.AddOptional("MATCH", Pattern);
        }

        /// <inheritdoc />
        protected override ScanResult<string> ConvertResponse(ArrayRedisValue responseValue)
        {
            var items = responseValue.Items;
            if(items.Count != 2)
                throw new InvalidOperationException("SCAN response should contains two items array");

            if (items[0] is BulkStringRedisValue strKey && items[1] is ArrayRedisValue arr)
                return new ScanResult<string>(int.Parse(strKey.Value), arr.Items.Select(itm => ((BulkStringRedisValue) itm).Value));
            
            throw new InvalidOperationException("Key or result array has wrong type");
        }
        
        
    }
}
