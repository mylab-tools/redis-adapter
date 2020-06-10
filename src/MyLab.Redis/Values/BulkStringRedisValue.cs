using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Redis.Values
{
    /// <summary>
    /// Represent Redis 'Bulk String' value
    /// </summary>
    public class BulkStringRedisValue : IStringRedisValue
    {
        /// <inheritdoc />
        public RedisValueType RedisType { get; } = RedisValueType.BulkString;

        /// <inheritdoc />
        public string Value { get; }
        
        /// <summary>
        /// Initializes a new instance of <see cref="BulkStringRedisValue"/>
        /// </summary>
        public BulkStringRedisValue(string value)
        {
            Value = value;
        }

        /// <inheritdoc />
        public async Task WriteAsync(Stream stream, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            var wrtr = new StreamWriter(stream);
            
            if (Value != null)
            {
                var bytesCount = encoding.GetByteCount(Value);

                await wrtr.WriteAsync(bytesCount + RedisValueTools.Separator);
                await wrtr.WriteAsync(Value);
            }
            else
            {
                await wrtr.WriteAsync("-1");
            }

            await wrtr.FlushAsync();
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

                string lenStr = await RedisValueTools.ReadLineAsync(stream, encoding);
                var len = int.Parse(lenStr);

                if (len != -1)
                {
                    var buff = new byte[len];
                    int readCount = 0;

                    do
                    {
                        int read = await stream.ReadAsync(buff, readCount, len - readCount);
                        readCount += read;

                    } while (readCount != len);

                    stream.ReadByte();
                    stream.ReadByte();

                    var str = encoding.GetString(buff);
                    return new BulkStringRedisValue(str);
                }

                return new BulkStringRedisValue(null);
            }
        }
    }
}