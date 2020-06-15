using System;
using System.Net.Sockets;
using System.Text;
using Nito.AsyncEx;

namespace MyLab.Redis.Connection
{
    public interface IRedisConnectionFactory
    {
        IRedisConnection Create(TcpClient client, IDisposable syncDisposer);
    }

    class DefaultRedisConnectionFactory : IRedisConnectionFactory
    {
        private readonly Encoding _encoding;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultRedisConnectionFactory"/>
        /// </summary>
        public DefaultRedisConnectionFactory( Encoding encoding)
        {
            _encoding = encoding;
        }
        public IRedisConnection Create(TcpClient client, IDisposable syncDisposer)
        {
            return new DefaultRedisConnection(client, syncDisposer)
            {
                Encoding = _encoding
            };
        }
    }
}