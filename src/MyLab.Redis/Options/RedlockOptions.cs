namespace MyLab.Redis.Options
{
    public class RedlockOptions
    {
        public int ExpirySec { get; set; } = 60;
        public int LockingTimeoutSec { get; set; } = 5;
        public int RetryPeriodSec { get; set; } = 1;
        public string BaseKey { get; set; } = "redlock";
        public int DbIndex { get; set; } = -1;
    }
}
