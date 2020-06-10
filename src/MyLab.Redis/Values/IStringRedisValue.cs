using System;
using System.Collections.Generic;
using System.Text;

namespace MyLab.Redis.Values
{
    /// <summary>
    /// Defines string Redis value
    /// </summary>
    public interface IStringRedisValue : IRedisValue
    {
        /// <summary>
        /// Simple string Redis value
        /// </summary>
        string Value { get; }
    }
}
