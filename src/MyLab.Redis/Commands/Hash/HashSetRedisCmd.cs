using System;
using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Hash
{
    /// <summary>
    /// HSET command
    /// </summary>
    public class HashSetRedisCmd : FuncRedisCommand<SetCmdResult, IntegerRedisValue>
    {
        /// <summary>
        /// Gets field name
        /// </summary>
        public string Field { get; }

        /// <summary>
        /// Gets field value
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="HashSetRedisCmd"/>
        /// </summary>
        public HashSetRedisCmd(string key, string field, string value)
            :base("HSET", key)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            parameters.Add(Field);
            parameters.Add(Value);
        }

        /// <inheritdoc />
        protected override SetCmdResult ConvertResponse(IntegerRedisValue responseValue)
        {
            switch (responseValue.Value)
            {
                case 0 : return SetCmdResult.Added;
                case 1 : return SetCmdResult.Updated;
                default: throw new InvalidOperationException("Unexpected response code: " + responseValue.Value);
            }
        }
    }
}
