using System;

namespace MyLab.Redis.Connection
{
    /// <summary>
    /// Throws when redis connection request has expired
    /// </summary>
    public class ConnectionRequestTimeoutException : RedisException
    {
        public ConnectionRequestTimeoutException(TimeSpan timeout) : base("Connection request expired in " + timeout.ToString("g"))
        {
        }
    }
}