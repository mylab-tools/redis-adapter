using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Redis.Values
{
    /// <summary>
    /// Represent Error Redis value
    /// </summary>
    public class ErrorRedisValue : IRedisValue
    {
        /// <inheritdoc />
        public RedisValueType RedisType { get; } = RedisValueType.Error;
         
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; }
        
        
        /// <inheritdoc />
        public bool FinishNewLineSelfControl { get; } = false;

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorRedisValue"/>
        /// </summary>
        public ErrorRedisValue(string message)
        {
            Message = message != null ? message.Replace(RedisValueTools.Separator, " ") : "The message is empty";
        }

        /// <inheritdoc />
        public async  Task WriteAsync(Stream stream, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            await RedisValueTools.WriteLineAsync(stream, Message, encoding);
        }
        
        /// <summary>
        /// Throws an exception
        /// </summary>
        public Exception ToException()
        {
            return new RedisException(Message);
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

                var line = await RedisValueTools.ReadLineAsync(stream, encoding);
                return new ErrorRedisValue(line);
            }
        }
    }
}