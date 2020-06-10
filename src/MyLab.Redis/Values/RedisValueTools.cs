using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Redis.Values
{
    static class RedisValueTools
    {
        public const string Separator = "\r\n";

        public static async Task<string> ReadLineAsync(Stream stream, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            if (Equals(encoding, Encoding.UTF8))
                encoding = new UTF8Encoding(false);

            var sb = new StringBuilder();
            var buff = new byte[1];
            var buffLast = new byte[2];
            bool hasEndOfString = false;

            byte[] resultBin;

            using (var mem = new MemoryStream())
            {
                do
                {
                    int readByte = await stream.ReadAsync(buff, 0, 1);
                    if (readByte == 0) break;

                    buffLast[0] = buffLast[1];
                    buffLast[1] = buff[0];

                    mem.WriteByte(buff[0]);

                    hasEndOfString = buffLast[0] == Separator[0] &&
                                     buffLast[1] == Separator[1];

                } while (!hasEndOfString);

                resultBin = mem.ToArray();
            }
            return encoding.GetString(resultBin, 0, hasEndOfString ? resultBin.Length - 2 : resultBin.Length);
        }

        public static async Task WriteLineAsync(Stream stream, string line, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (line == null) throw new ArgumentNullException(nameof(line));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            if (Equals(encoding, Encoding.UTF8))
                encoding = new UTF8Encoding(false);

            var wrtr = new StreamWriter(stream, encoding);
            await wrtr.WriteAsync(line);
            await wrtr.FlushAsync();
        }
    }
}
