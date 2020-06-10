using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Redis.Values
{
    /// <summary>
    /// Provides extensions for <see cref="IRedisValue"/>
    /// </summary>
    public static class RedisValueExtensions
    {
        /// <summary>
        /// Converts Redis value to string
        /// </summary>
        public static async Task<string> SerializeAsync(this IRedisValue value, Encoding encoding = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            string str;
            var enc = encoding ?? Encoding.Default;

            await using (var mem = new MemoryStream())
            {
                await value.WriteAsync(mem, enc);
                str = enc.GetString(mem.ToArray());
            }

            return str;
        }

        /// <summary>
        /// Writes value to stream
        /// </summary>
        public static async Task WriteAsync(this IRedisValue value, Stream stream)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            await value.WriteAsync(stream, Encoding.Default);
        }
    }
}
