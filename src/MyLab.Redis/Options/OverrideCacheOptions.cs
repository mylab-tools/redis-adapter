namespace MyLab.Redis.Options;

public class OverrideCacheOptions : CacheOptions
{

    public required string Id { get; set; }

    public CacheOptions Override(CacheOptions origin)
    {
        return new CacheOptions
        {
            ExpirySec = origin.ExpirySec == Default.ExpirySec ? ExpirySec : origin.ExpirySec,
            DbIndex = origin.DbIndex == Default.DbIndex ? DbIndex : origin.DbIndex,
            BaseKey = origin.BaseKey == Default.BaseKey ? BaseKey : origin.BaseKey
        };
    }
}