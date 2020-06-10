using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Redis.Values
{
    /// <summary>
    /// Reads Redis value from stream
    /// </summary>
    public class ValuesStreamReader
    {
        private readonly Stream _stream;

        /// <summary>
        /// Encoding to read values
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ValuesStreamReader"/>
        /// </summary>
        public ValuesStreamReader(Stream stream)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }
        /// <summary>
        /// Read value from stream
        /// </summary>
        public async Task<IRedisValue> ReadValueAsync()
        {
            var enc = Encoding ?? Encoding.Default;

            var hb = await ReadHeaderByteAsync();

            if (!RedisValueReaders.Instance.TryGetValue(hb, out var reader))
            {
                throw new NotSupportedException("Value type not supported");
            }

            return await reader.ReadAsync(_stream, enc);
        }

        async Task<byte> ReadHeaderByteAsync()
        {
            var buff = new byte[1];
            var read = await _stream.ReadAsync(buff, 0, 1);

            if (read != 1)
                throw new InvalidOperationException("No data to read");

            return buff[0];
        }
    }
}
