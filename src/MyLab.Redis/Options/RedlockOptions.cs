namespace MyLab.Redis.Options
{
    /// <summary>
    /// Settings of a distributed lock.
    /// </summary>
    public class RedlockOptions
    {
        /// <summary>
        /// How long a taken lock is kept, in seconds, if it is not released.
        /// </summary>
        public int ExpirySec { get; set; } = 60;

        /// <summary>
        /// How long an acquire attempt waits, in seconds.
        /// </summary>
        public int LockingTimeoutSec { get; set; } = 5;

        /// <summary>
        /// Pause between unsuccessful acquire attempts, in seconds.
        /// </summary>
        public int RetryPeriodSec { get; set; } = 1;

        /// <summary>
        /// Prefix of lock keys. A lock key is <c>{BaseKey}:{lockId}</c>.
        /// </summary>
        public string BaseKey { get; set; } = "redlock";

        /// <summary>
        /// Database index. A negative value selects the configured default database.
        /// </summary>
        public int DbIndex { get; set; } = -1;
    }
}
