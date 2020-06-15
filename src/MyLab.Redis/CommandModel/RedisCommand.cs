using System;
using System.Collections.Generic;
using System.Linq;
using MyLab.Redis.Values;

namespace MyLab.Redis.CommandModel
{
    /// <summary>
    /// Contains redis command name and parameters
    /// </summary>
    public abstract class RedisCommand

    {
        /// <summary>
        /// Gets a command name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a key name 
        /// </summary>
        /// <remarks>Optional</remarks>
        public string Key { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="RedisCommand"/>
        /// </summary>
        protected RedisCommand(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            Name = name.ToUpper();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RedisCommand"/>
        /// </summary>
        protected RedisCommand(string name, string key)
            : this(name)
        {
            Key = key;
        }

        /// <summary>
        /// Gets command parameters
        /// </summary>
        protected virtual void GetParameters(CommandParameters parameters) { }


        public ArrayRedisValue ToRedisArray()
        {
            var vals = new List<IRedisValue> { new BulkStringRedisValue(Name) };

            if (!string.IsNullOrWhiteSpace(Key))
                vals.Add(new BulkStringRedisValue(Key));

            var p = new CommandParameters();
            GetParameters(p);
            vals.AddRange(p.ToArray().Select(s => new BulkStringRedisValue(s)));

            return new ArrayRedisValue(vals);
        }
    }
}
