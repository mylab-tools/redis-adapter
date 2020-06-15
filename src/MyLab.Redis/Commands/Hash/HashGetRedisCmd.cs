using System;
using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Hash
{
    /// <summary>
    /// HGET command
    /// </summary>
    public class HashGetRedisCmd : FuncRedisCommand<string, BulkStringRedisValue>
    {
        /// <summary>
        /// Gets field name
        /// </summary>
        public string Field { get; }

        /// <inheritdoc />
        public HashGetRedisCmd(string key, string field) : base("HGET", key)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }

        /// <inheritdoc />
        protected override string ConvertResponse(BulkStringRedisValue responseValue)
        {
            return responseValue.Value;
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            parameters.Add(Field);
        }
    }
}
