namespace MyLab.Redis
{
    /// <summary>
    /// Contains Redis connection options
    /// </summary>
    public class RedisOptions
    {
        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Port. 6379 by default.
        /// </summary>
        public short Port { get; set; } = 6379;
        /// <summary>
        /// Data base index. 0 by default.
        /// </summary>
        public short DbIndex { get; set; }
        /// <summary>
        /// Password. Disabled (null) by default.
        /// </summary>
        public string Password{ get; set; }

        /// <summary>
        /// Timeout in seconds until connection will be provided. 30 by default.
        /// </summary>
        public int ConnectionRequestTimeout { get; set; } = 30;

        /// <summary>
        /// Encoding. UTF8 bu default.
        /// </summary>
        public string Encoding { get; set; }
    }
}