using System;
using System.Collections.Generic;
using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Hash
{
    /// <summary>
    /// HGETALL command
    /// </summary>
    public class HashGetAllRedisCmd : FuncRedisCommand<IDictionary<string, IRedisValue>, ArrayRedisValue>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HashGetAllRedisCmd"/>
        /// </summary>
        public HashGetAllRedisCmd(string key) : base("HGETALL", key)
        {
        }

        /// <inheritdoc />
        protected override IDictionary<string, IRedisValue> ConvertResponse(ArrayRedisValue responseValue)
        {
            var items = responseValue.Items;
            if (items.Count % 2 != 0) throw new InvalidOperationException("Odd number of elements. Hash should has pairs with key-value model");

            var dict = new Dictionary<string, IRedisValue>();
            
            for (int i = 0; i < items.Count; i += 2)
            {
                if(items[i] is BulkStringRedisValue strKey)
                    dict.Add(strKey.Value, items[i + 1]);
                else
                {
                    throw new InvalidOperationException("Key is not string");
                }
            }

            return dict;
        }
    }
}
