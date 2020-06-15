using System;
using System.Globalization;
using MyLab.Redis.CommandModel;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands.Strings
{
    /// <summary>
    /// SET redis command
    /// </summary>
    public class SetRedisCmd : FuncRedisCommand<bool, IStringRedisValue>
    {
        /// <summary>
        /// Gets string value
        /// </summary>
        public string Value { get; }
        
        /// <summary>
        /// Expire time
        /// </summary>
        public TimeSpan ExpireTime { get; set; } = TimeSpan.Zero;
        
        /// <summary>
        /// Performing condition
        /// </summary>
        public ConditionMode Condition { get; set; }
        
        /// <inheritdoc />
        public SetRedisCmd(string key, string value) : base("SET", key)
        {
            Value = value;
        }

        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            parameters.Add(Value);

            switch (Condition)
            {
                case ConditionMode.OnlyIfExists:
                    parameters.Add("XX");
                    break;
                case ConditionMode.OnlyIfNotExists:
                    parameters.Add("NX");
                    break;
            }
            
            if(ExpireTime != TimeSpan.Zero)
                parameters.Add("PX", ExpireTime.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Set command performing mode
        /// </summary>
        public enum ConditionMode
        {
            /// <summary>
            /// Undefined
            /// </summary>
            Undefined,
            /// <summary>
            /// Only set the key if it already exist
            /// </summary>
            OnlyIfExists,
            /// <summary>
            /// Only set the key if it does not already exist
            /// </summary>
            OnlyIfNotExists
        }

        /// <inheritdoc />
        protected override bool ConvertResponse(IStringRedisValue responseValue)
        {   
            return responseValue.Value == "OK";
        }
    }
}
