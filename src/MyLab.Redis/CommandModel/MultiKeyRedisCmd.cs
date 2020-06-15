using System;
using System.Collections.Generic;
using MyLab.Redis.Values;

namespace MyLab.Redis.CommandModel
{
    /// <summary>
    /// Multi key command command
    /// </summary>
    public class MultiKeyRedisCmd : FuncRedisCommand<int, IntegerRedisValue>
    {
        readonly List<string> _keys = new List<string>();

        /// <summary>
        /// Gets keys for delete
        /// </summary>
        public IReadOnlyList<string> Keys { get; }


        MultiKeyRedisCmd(string name)
            : base(name)
        {
            Keys = _keys.AsReadOnly();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MultiKeyRedisCmd"/>
        /// </summary>
        protected MultiKeyRedisCmd(string name, string key, params string[] keys) : this(name)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

            _keys.Add(key);
            if (keys != null)
                _keys.AddRange(keys);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MultiKeyRedisCmd"/>
        /// </summary>
        public MultiKeyRedisCmd(string name, IEnumerable<string> initialKeys) : this(name)
        {
            _keys.AddRange(initialKeys);
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            if (_keys.Count == 0)
                throw new InvalidOperationException("No keys specified");

            parameters.AddRange(_keys);
        }

        /// <inheritdoc />
        protected override int ConvertResponse(IntegerRedisValue responseValue)
        {
            return responseValue.Value;
        }
    }
}