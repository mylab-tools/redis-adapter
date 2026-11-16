namespace MyLab.Redis.Options;

public class CacheOptions
{
    public static readonly CacheOptions Default = new();
    public int ExpirySec { get; set; } = 0;
    public int DbIndex { get; set; } = -1;
    public string BaseKey { get; set; } = "cache";
    public string LockId { get; set; } = "cache";
}