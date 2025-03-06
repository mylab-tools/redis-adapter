namespace MyLab.Redis.Options
{
    /// <summary>
    /// Contains licking options
    /// </summary>
    public class LockingOptions
    {
        /// <summary>
        /// Gets Redis-key name prefix
        /// </summary>
        public string? KeyPrefix { get; set; } = "redlock";

        /// <summary>
        /// Gets named lock options
        /// </summary>
        public LockOptions[]? Locks { get; set; }
    }
}