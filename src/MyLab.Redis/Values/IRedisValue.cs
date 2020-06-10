using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Redis.Values
{
    /// <summary>
    /// Redis value
    /// </summary>
    public interface IRedisValue
    {
        /// <summary>
        /// Gets redis value type
        /// </summary>
        RedisValueType RedisType { get; }

        /// <summary>
        /// Writes value to stream
        /// </summary>
        Task WriteAsync(Stream stream, Encoding encoding);
    }
}
