using System;
using System.Net.Sockets;

namespace MyLab.Redis.Connection
{
    /// <summary>
    /// Provides <see cref="TcpClient"/>
    /// </summary>
    public interface ITcpClientProvider  : IDisposable
    {
        /// <summary>
        /// Provides <see cref="TcpClient"/>
        /// </summary>
        TcpClient Provide(out bool isNew);
    }

    class DefaultTcpClientProvider : ITcpClientProvider
    {
        private readonly string _host;
        private readonly short _port;

        private TcpClient _currentClient;

        public DefaultTcpClientProvider(string host, short port)
        {
            _host = host;
            _port = port;
        }

        public TcpClient Provide(out bool isNew)
        {
            if (_currentClient == null || !_currentClient.Connected)
            {
                _currentClient?.Dispose();
                _currentClient = new TcpClient(_host, _port);

                isNew = true;
            }
            else
            {
                isNew = false;
            }

            return _currentClient;
        }

        public void Dispose()
        {
            _currentClient?.Dispose();
        }
    }
}