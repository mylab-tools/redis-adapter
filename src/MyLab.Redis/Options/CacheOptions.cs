namespace MyLab.Redis.Options;

public class CacheOptions
{
    public required string Name { get; set; }
    public int ExpirySec { get; set; } = 0;
}