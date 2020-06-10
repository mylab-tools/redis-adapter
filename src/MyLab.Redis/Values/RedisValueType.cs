namespace MyLab.Redis.Values
{
    /// <summary>
    /// Enums Redis value types
    /// </summary>
    public enum RedisValueType
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined,
        /// <summary>
        /// Single string value
        /// </summary>
        String,
        /// <summary>
        /// Integer value
        /// </summary>
        Integer,
        /// <summary>
        /// Bulk string value
        /// </summary>
        BulkString,
        /// <summary>
        /// Array value
        /// </summary>
        Array,
        /// <summary>
        /// Error
        /// </summary>
        Error
    }
}
