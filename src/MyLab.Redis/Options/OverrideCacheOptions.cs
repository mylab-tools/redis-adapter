namespace MyLab.Redis.Options;

/// <summary>
/// Cache settings that apply to one cache identifier.
/// </summary>
public class OverrideCacheOptions : CacheOptions
{
    /// <summary>
    /// Cache identifier these settings apply to.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Returns <paramref name="origin"/> with values replaced by this override where the origin still has the default.
    /// </summary>
    /// <param name="origin">Settings being overridden.</param>
    /// <returns>The combined settings.</returns>
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
