using System;

namespace MyLab.Redis.Connection
{
    /// <summary>
    /// Occurred when Redis connection is not established
    /// </summary>
    public class RedisNotConnectedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RedisNotConnectedException"/>
        /// </summary>
        public RedisNotConnectedException() : base("The Radis connection is not established")
        {

        }
    }
}