using System.Net.Sockets;

namespace MyLab.Redis.Connection
{
    /// <summary>
    /// Provides <see cref="TcpClient"/>
    /// </summary>
    public interface ITcpClientProvider
    {
        /// <summary>
        /// Provides <see cref="TcpClient"/>
        /// </summary>
        TcpClient Provide();
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

        public TcpClient Provide()
        {
            if (_currentClient == null || !_currentClient.Connected)
            {
                _currentClient?.Dispose();
                _currentClient = new TcpClient(_host, _port);
            }

            return _currentClient;
        }
    }
}