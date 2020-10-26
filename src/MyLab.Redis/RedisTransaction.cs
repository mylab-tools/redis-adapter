using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis
{
    /// <summary>
    /// Represent Redis transaction
    /// </summary>
    public class RedisTransaction : RedisDbKeysProvider, IAsyncDisposable
    {
        private readonly ITransaction _transaction;

        /// <summary>
        /// Initializes a new instance of <see cref="RedisTransaction"/>
        /// </summary>
        public RedisTransaction(ITransaction transaction)
            : base(transaction)
        {
            _transaction = transaction;
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(_transaction. ExecuteAsync());
        }
    }
}