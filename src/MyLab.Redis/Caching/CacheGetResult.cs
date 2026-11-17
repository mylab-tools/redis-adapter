namespace MyLab.Redis.Caching;

/// <summary>
/// Value returned by a cache read.
/// </summary>
/// <typeparam name="T">Type of the cached value.</typeparam>
/// <param name="Hit"><see langword="true"/> when the value was read from cache or stored successfully.</param>
/// <param name="Value">The cached value, or the default value when the key is missing.</param>
public record CacheGetResult<T>(bool Hit, T Value)
{
    public static readonly CacheGetResult<T> Default = new(false, default);
}