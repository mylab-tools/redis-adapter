using System;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyLab.Redis.Values;
using Nito.AsyncEx;

namespace MyLab.Redis.Connection
{
    public interface IRedisConnection : IDisposable
    {
        Task<IRedisValue> PerformCommandAsync(ArrayRedisValue command);
    }

    class DefaultRedisConnection : IRedisConnection
    {
        private readonly TcpClient _tcpClient;
        private readonly IDisposable _syncDisposer;

        public Encoding Encoding { get; set; }

        public DefaultRedisConnection(
            TcpClient tcpClient,
            IDisposable syncDisposer)
        {
            _tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            _syncDisposer = syncDisposer ?? throw new ArgumentNullException(nameof(syncDisposer));
        }

        public async Task<IRedisValue> PerformCommandAsync(ArrayRedisValue command)
        {
            var wrtr = new ValuesStreamWriter(_tcpClient.GetStream())
            {
                Encoding = Encoding
            };

            await wrtr.WriteAsync(command);

            var rdr = new ValuesStreamReader(_tcpClient.GetStream())
            {
                Encoding = Encoding
            };

            return await rdr.ReadValueAsync();
        }

        public void Dispose()
        {
            _syncDisposer.Dispose();
        }
    }
}
