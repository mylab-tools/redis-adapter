namespace MyLab.Redis.Caching;

/// <summary>
/// Loads a cache item when its key is missing.
/// </summary>
/// <typeparam name="T">Type of the loaded value.</typeparam>
/// <param name="itemId">Identifier of the missing item.</param>
/// <returns>The value to store in the cache.</returns>
public delegate Task<T> AsyncItemProvider<T>(string itemId);