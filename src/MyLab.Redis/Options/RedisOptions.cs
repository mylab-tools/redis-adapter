namespace MyLab.Redis.Options
{
    public class RedisOptions
    {
        public const string SectionName = "Redis";
        public bool Optional { get; set; } = false;
        public string? ConnectionString { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
        public ConfigCacheOptions? Cache { get; set; }
        public RedlockOptions? Redlock { get; set; }
    }
}
