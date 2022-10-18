using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MyLab.Redis.Scripting;

namespace MyLab.Redis.ObjectModel
{
    public class Redlocker
    {
        private readonly RedisDbProvider _dbProvider;

        /// <summary>
        /// Gets lock name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets a lock expiry
        /// </summary>
        public TimeSpan Expiry { get; set; }

        /// <summary>
        /// Determines the timeout for a locking attempt
        /// </summary>
        public TimeSpan DefaultLockingTimeout { get; set; }

        /// <summary>
        /// Determines a waiting period between locking attempts
        /// </summary>
        public TimeSpan RetryPeriod { get; set; }


        /// <summary>
        /// Initializes a new instance of <see cref="Redlock"/>
        /// </summary>
        public Redlocker(RedisDbProvider dbProvider, string name)
        {
            _dbProvider = dbProvider;
            Name = name;
        }

        /// <summary>
        /// Returns true if state is locked by someone
        /// </summary>
        public async Task<bool> IsAcquiredAsync()
        {
            var keyExists = await _dbProvider.Provide().KeyExistsAsync(Name);

            return keyExists;
        }

        /// <summary>
        /// Tries to lock the state once
        /// </summary>
        public Task<LockAttempt> TryLockOnceAsync()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            return TryLockAsync(cts.Token);
        }

        /// <summary>
        /// Tries to lock the state with default timeout
        /// </summary>
        public Task<LockAttempt> TryLockAsync()
        {
            return TryLockAsync(DefaultLockingTimeout);
        }

        /// <summary>
        /// Tries to lock the state with specified timeout
        /// </summary>
        public Task<LockAttempt> TryLockAsync(TimeSpan timeout)
        {
            var cts = new CancellationTokenSource(timeout);
            return TryLockAsync(cts.Token);
        }

        /// <summary>
        /// Tries to lock the state with <see cref="CancellationToken"/>
        /// </summary>
        public async Task<LockAttempt> TryLockAsync(CancellationToken cancellationToken)
        {
            var key = new StringRedisKey(_dbProvider, Name);
            var id = Guid.NewGuid().ToString("N");

            bool successLock;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            do
            {
                successLock = await key.SetIfNotExistsAsync(id, Expiry);

                if (!successLock)
                {
                    try
                    {
                        await Task.Delay(RetryPeriod, cancellationToken);
                    }
                    catch (TaskCanceledException)
                    {
                    }
                }

            } while (!successLock && !cancellationToken.IsCancellationRequested);

            var resultRedlock = successLock 
                ? new Redlock(key, id, new RedisScriptTools(_dbProvider)) {DefaultProlongSpan = Expiry}
                : null;
            return new LockAttempt(resultRedlock, successLock);
        }
    }
}