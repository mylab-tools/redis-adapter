using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Redis.Values
{
    public class ValuesStreamWriter
    {
        private readonly Stream _stream;

        /// <summary>
        /// Gets or sets encoding
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ValuesStreamWriter"/>
        /// </summary>
        public ValuesStreamWriter(Stream stream)
        {
            _stream = stream;
        }

        /// <summary>
        /// Writes Redis value to stream
        /// </summary>
        public async Task WriteAsync(IRedisValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            byte[] firstByteBuff =
            {
                RedisValueTypeTools.RedisTypeToHeaderByte(value.RedisType)
            };
            var enc = Encoding ?? Encoding.Default;

            await _stream.WriteAsync(firstByteBuff, 0, 1);
            await value.WriteAsync(_stream, enc);

            if (value.RedisType != RedisValueType.Array)
            {
                var newLineBin = enc.GetBytes(RedisValueTools.Separator);
                await _stream.WriteAsync(newLineBin, 0, newLineBin.Length);
            }

            await _stream.FlushAsync();
        }
    }
}