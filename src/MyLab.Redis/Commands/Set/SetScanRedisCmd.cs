using System;
using System.Linq;
using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Set
{
    /// <summary>
    /// SSCAN redis command
    /// </summary>
    public class SetScanRedisCmd<T> : FuncRedisCommand<ScanResult<T>, ArrayRedisValue>, IRedisScanCmd<T>
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
        public SetScanRedisCmd(int cursor, string key) : base("SSCAN", key)
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
        protected override ScanResult<T> ConvertResponse(ArrayRedisValue responseValue)
        {
            var items = responseValue.Items;
            if(items.Count != 2)
                throw new InvalidOperationException("SSCAN response should contains two items array");

            if (items[0] is BulkStringRedisValue strKey && items[1] is ArrayRedisValue arr)
                return new ScanResult<T>(int.Parse(strKey.Value), 
                    arr.Items.Select(itm => ValueParsingTools.ParseCleanString<T>(((BulkStringRedisValue) itm).Value)));
            
            throw new InvalidOperationException("Key or result array has wrong type");
        }
    }
}
