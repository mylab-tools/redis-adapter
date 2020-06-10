using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Redis.Values
{
    /// <summary>
    /// Represent Redis 'Simple String' value
    /// </summary>
    public class StringRedisValue : IStringRedisValue
    {
        /// <summary>
        /// Simple string Redis value
        /// </summary>
        public string Value { get; }

        /// <inheritdoc />
        public RedisValueType RedisType { get; } = RedisValueType.String;
        
        /// <summary>
        /// Initializes a new instance of <see cref="StringRedisValue"/>
        /// </summary>
        public StringRedisValue(string value)
        {
            Value = value != null ? value.Replace(RedisValueTools.Separator, " ") : string.Empty;
        }

        /// <inheritdoc />
        public async Task WriteAsync(Stream stream, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            await RedisValueTools.WriteLineAsync(stream, Value, encoding);
        }
        
        /// <summary>
        /// Creates value from <see cref="DateTime"/>
        /// </summary>
        public static StringRedisValue FromDateTime(DateTime dateTime) => new StringRedisValue(dateTime.ToString("R"));
        
        /// <summary>
        /// Creates value from <see cref="Guid"/>
        /// </summary>
        public static StringRedisValue FromGuid(Guid guid) => new StringRedisValue(guid.ToString("N"));
        
        /// <summary>
        /// Creates value from <see cref="object"/>
        /// </summary>
        public static StringRedisValue FromObject(object obj) => new StringRedisValue(obj != null ? obj.ToString() : string.Empty);

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

                var line = await RedisValueTools.ReadLineAsync(stream, encoding);
                return new StringRedisValue(line);
            }
        }
    }
}