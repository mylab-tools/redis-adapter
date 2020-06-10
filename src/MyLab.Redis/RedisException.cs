using System;
using System.Collections.Generic;
using System.Text;

namespace MyLab.Redis
{
    /// <summary>
    /// Contains Redis exception
    /// </summary>
    public class RedisException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RedisException"/>
        /// </summary>
        public RedisException(string message)
            : base(message)
        {

        }
    }
}
