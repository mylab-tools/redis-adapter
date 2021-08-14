//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using StackExchange.Redis;

//namespace MyLab.Redis
//{
//    /// <summary>
//    /// Represent Redis transaction
//    /// </summary>
//    public class RedisTransaction : IAsyncDisposable
//    {
//        private readonly ITransaction _transaction;
//        private readonly RedisDbKeysProvider _db;
//        private readonly List<Task> _tasks;

//        /// <summary>
//        /// Initializes a new instance of <see cref="RedisTransaction"/>
//        /// </summary>
//        public RedisTransaction(ITransaction transaction)
//            :this(transaction, new RedisDbKeysProvider(transaction), new List<Task>())
            
//        {
//        }

//        RedisTransaction(ITransaction transaction, RedisDbKeysProvider db, List<Task> initialTasks)

//        {
//            _transaction = transaction;
//            _db = db;
//            _tasks = initialTasks;
//        }

//        public RedisTransaction Enqueue(Func<RedisDbKeysProvider, Task> cmd)
//        {
//            return new RedisTransaction(_transaction, _db, new List<Task>(_tasks) { cmd(_db) });
//        }

//        public async ValueTask DisposeAsync()
//        {

//            if (await _transaction.ExecuteAsync())
//                await Task.WhenAll(_tasks);
//        }
//    }
//}