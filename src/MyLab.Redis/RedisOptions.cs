using System;

namespace MyLab.Redis
{
    /// <summary>
    /// Contains Redis configuration
    /// </summary>
    public class RedisOptions 
    {
        /// <summary>
        /// Connection string
        /// </summary>
        /// <remarks>https://stackexchange.github.io/StackExchange.Redis/Configuration</remarks>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Overrides password from <see cref="ConnectionString"/>
        /// </summary>
        public string Password { get; set; }

        public Action<RedisOptions> CreateCopyAction()
        {
            return o =>
            {
                o.ConnectionString = ConnectionString;
                o.Password = Password;
            };
        }
    }
}
