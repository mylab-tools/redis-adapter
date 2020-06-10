using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Redis.Values
{
    /// <summary>
    /// Represent Redis 'Integer' value
    /// </summary>
    public class IntegerRedisValue : IRedisValue
    {
        /// <summary>
        /// Integer Redis value
        /// </summary>
        public int Value { get; }
        
        /// <inheritdoc />
        public RedisValueType RedisType { get; } = RedisValueType.Integer;

        /// <summary>
        /// Initializes a new instance of <see cref="IntegerRedisValue"/>
        /// </summary>
        public IntegerRedisValue(int value)
        {
            Value = value;
        }

        /// <inheritdoc />
        public async  Task WriteAsync(Stream stream, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            await RedisValueTools.WriteLineAsync(stream, Value.ToString(), encoding);
        }

        public static IRedisValueReader CreateReader()
        {
            return new Reader();
        }

        class Reader : IRedisValueReader
        {
            public async Task<IRedisValue> ReadAsync(Stream stream, Encoding encoding)
            {
                if (stream == null) throw new ArgumentNullException(nameof(stream));
                if (encoding == null) throw new ArgumentNullException(nameof(encoding));

                var strVal = await RedisValueTools.ReadLineAsync(stream, encoding);
                return new IntegerRedisValue(int.Parse(strVal));
            }
        }
    }
}