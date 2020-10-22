using System;

namespace MyLab.Redis
{
    /// <summary>
    /// Occurs when operation could not be performed
    /// </summary>
    public class RedisOperationException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RedisOperationException"/>
        /// </summary>
        public RedisOperationException(string message)
            :base(message)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RedisOperationException"/>
        /// </summary>
        public RedisOperationException()
            : this("Could not perform operation")
        {

        }
    }
}