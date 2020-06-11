using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MyLab.Redis.Values;

namespace MyLab.Redis.Connection
{
    public interface IRedisConnection : IDisposable
    {
        Task<ArrayRedisValue> PerformCommand(ArrayRedisValue command);
    }

    class DefaultRedisConnection : IRedisConnection
    {
        private readonly TcpClient _tcpClient;
        private readonly object _sync;

        public DefaultRedisConnection(TcpClient tcpClient, object sync)
        {
            _tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            _sync = sync ?? throw new ArgumentNullException(nameof(sync));
        }

        public Task<ArrayRedisValue> PerformCommand(ArrayRedisValue command)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Monitor.Exit(_sync);
        }
    }
}
