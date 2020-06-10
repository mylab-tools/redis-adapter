using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Redis.Values
{
    /// <summary>
    /// Represent Redis array value
    /// </summary>
    public class ArrayRedisValue : IRedisValue
    {
        /// <summary>
        /// Array items
        /// </summary>
        public IReadOnlyList<IRedisValue> Items { get; }

        /// <inheritdoc />
        public RedisValueType RedisType { get; } = RedisValueType.Array;

        /// <summary>
        /// Initializes a new instance of <see cref="ArrayRedisValue"/>
        /// </summary>
        public ArrayRedisValue(IEnumerable<IRedisValue> initial)
        {
            if(initial != null)
                Items = new List<IRedisValue>(initial).AsReadOnly();
        }

        public async Task WriteAsync(Stream stream, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            
            var wrtr = new StreamWriter(stream);

            if (Items != null)
            {
                await wrtr.WriteAsync(Items.Count.ToString());
                await wrtr.WriteAsync(RedisValueTools.Separator);
                await wrtr.FlushAsync();

                var valWrtr = new ValuesStreamWriter(stream)
                {
                    Encoding = encoding
                };

                foreach (var item in Items)
                {
                    await valWrtr.WriteAsync(item);
                }
            }
            else
            {
                await wrtr.WriteAsync("-1" + RedisValueTools.Separator);
                await wrtr.FlushAsync();
            }
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
                    var vRdr = new ValuesStreamReader(stream) { Encoding = encoding };
                    var lst = new List<IRedisValue>();

                    for (int i = 0; i < len; i++)
                    {
                        lst.Add(await vRdr.ReadValueAsync());
                    }

                    return new ArrayRedisValue(lst);
                }

                return new ArrayRedisValue(null);
            }
        }
    }
}