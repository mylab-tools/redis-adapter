namespace MyLab.Redis.Caching;

public delegate Task<T> AsyncItemProvider<T>(string itemId);