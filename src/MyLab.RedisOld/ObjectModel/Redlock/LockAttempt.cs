using System;
using System.Threading.Tasks;

namespace MyLab.Redis.ObjectModel
{
    /// <summary>
    /// Contains locking attempt properties
    /// </summary>
    public class LockAttempt : IAsyncDisposable
    {
        /// <summary>
        /// True - if locking success completed and state was acquired
        /// </summary>
        public bool Acquired { get; }

        /// <summary>
        /// Lock object
        /// </summary>
        public Redlock Lock { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="LockAttempt"/>
        /// </summary>
        public LockAttempt(Redlock @lock, bool acquired)
        {
            Lock = @lock;
            Acquired = acquired;
        }

        /// <inheritdoc />
        public ValueTask DisposeAsync()
        {
            return Lock?.DisposeAsync() ?? new ValueTask();
        }
    }
}