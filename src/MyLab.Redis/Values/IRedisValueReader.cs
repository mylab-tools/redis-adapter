using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Redis.Values
{
    /// <summary>
    /// Specifies Redis value reader
    /// </summary>
    public interface IRedisValueReader
    {
        /// <summary>
        /// Reads value from stream async
        /// </summary>
        Task<IRedisValue> ReadAsync(Stream stream, Encoding encoding);
    }

    class RedisValueReaders : ReadOnlyDictionary<byte, IRedisValueReader>
    {
        public static readonly RedisValueReaders Instance = new RedisValueReaders();

        /// <summary>
        /// Initializes a new instance of <see cref="RedisValueReaders"/>
        /// </summary>
        RedisValueReaders()
            :base(RegisterReaders())
        {
            
        }

        private static IDictionary<byte, IRedisValueReader> RegisterReaders()
        {
            return new Dictionary<byte, IRedisValueReader>
            {
                { RedisValueTypeTools.RedisTypeToHeaderByte(RedisValueType.Array), ArrayRedisValue.CreateReader()},
                { RedisValueTypeTools.RedisTypeToHeaderByte(RedisValueType.BulkString), BulkStringRedisValue.CreateReader()},
                { RedisValueTypeTools.RedisTypeToHeaderByte(RedisValueType.Error), ErrorRedisValue.CreateReader()},
                { RedisValueTypeTools.RedisTypeToHeaderByte(RedisValueType.Integer), IntegerRedisValue.CreateReader()},
                { RedisValueTypeTools.RedisTypeToHeaderByte(RedisValueType.String), StringRedisValue.CreateReader()}
            };
        }
    }
}
