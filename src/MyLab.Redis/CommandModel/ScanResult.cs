using System;
using System.Collections.Generic;
using System.Text;

namespace MyLab.Redis.CommandModel
{
    /// <summary>
    /// Scan result
    /// </summary>
    public class ScanResult<T>
    {
        /// <summary>
        /// gets new cursor to continue search
        /// </summary>
        public int NewCursor { get; }

        /// <summary>
        /// Gets found items
        /// </summary>
        public IEnumerable<T> Items { get; }

        /// <summary>
        /// Gets TRUE if scan has completed
        /// </summary>
        public bool IsTheEnd => NewCursor == 0;

        /// <summary>
        /// Initializes a new instance of <see cref="ScanResult{T}"/>
        /// </summary>
        internal ScanResult(int newCursor, IEnumerable<T> items)
        {
            NewCursor = newCursor;
            Items = items;
        }
    }
}
