using System;
using System.Collections.Generic;
using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.List
{
    /// <summary>
    /// The base for list commands with items parameters
    /// </summary>
    public class ListItemsRedisCmd : FuncRedisCommand<int, IntegerRedisValue>
    {
        /// <summary>
        /// Gets list items
        /// </summary>
        public List<string> Items { get; } = new List<string>();

        /// <summary>
        /// Initializes a new instance of <see cref="ListRightPushRedisCmd"/>
        /// </summary>
        protected ListItemsRedisCmd(string name, string key) : base(name, key)
        {

        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            if(Items.Count == 0)
                throw new InvalidOperationException("Items list is empty");

            parameters.AddRange(Items);
        }

        /// <inheritdoc />
        protected override int ConvertResponse(IntegerRedisValue responseValue)
        {
            return responseValue.Value;
        }
    }
}