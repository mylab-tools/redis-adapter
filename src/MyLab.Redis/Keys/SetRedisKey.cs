using StackExchange.Redis;

namespace MyLab.Redis.Keys;

/// <summary>
/// Represent Redis SET
/// </summary>
public class SetRedisKey : SortedRedisKeyBase
{
    /// <summary>
    /// Initializes a new instance of <see cref="SetRedisKey"/>
    /// </summary>
    public SetRedisKey(string keyName, IDatabaseAsync redisDb)
        : base(keyName, redisDb)
    {
    }

    /// <summary>
    /// Add the specified member to the set stored at key.
    /// Specified members that are already a member of this set are ignored.
    /// If key does not exist, a new set is created before adding the specified members.
    /// </summary>
    /// <param name="value">The value to add to the set.</param>
    /// <returns>True if the specified member was not already present in the set, else False</returns>
    public Task<bool> AddAsync(RedisValue value)
        => RedisDb.SetAddAsync(KeyName, value);

    /// <summary>
    /// Add the specified members to the set stored at key.
    /// Specified members that are already a member of this set are ignored.
    /// If key does not exist, a new set is created before adding the specified members.
    /// </summary>
    /// <param name="values">The values to add to the set.</param>
    /// <returns>The number of elements that were added to the set, not including all the elements already present into the set.</returns>
    public Task<long> AddAsync(RedisValue[] values)
        => RedisDb.SetAddAsync(KeyName, values);

    /// <summary>
    /// Returns if member is a member of the set stored at key.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <returns><see langword="true"/> if the element is a member of the set. <see langword="false"/> if the element is not a member of the set, or if key does not exist.</returns>
    public Task<bool> ContainsAsync(RedisValue value)
        => RedisDb.SetContainsAsync(KeyName, value);

    /// <summary>
    /// Returns the set cardinality (number of elements) of the set stored at key.
    /// </summary>
    /// <returns>The cardinality (number of elements) of the set, or 0 if key does not exist.</returns>
    public Task<long> LengthAsync()
        => RedisDb.SetLengthAsync(KeyName);

    /// <summary>
    /// Returns all the members of the set value stored at key.
    /// </summary>
    /// <returns>All elements of the set.</returns>
    public Task<RedisValue[]> MembersAsync()
        => RedisDb.SetMembersAsync(KeyName);

    /// <summary>
    /// Move member from the set at source to the set at destination. This operation is atomic.
    /// In every given moment the element will appear to be a member of source or destination for other clients.
    /// When the specified element already exists in the destination set, it is only removed from the source set.
    /// </summary>
    /// <param name="destination">The key of the destination set.</param>
    /// <param name="value">The value to move.</param>
    /// <returns>1 if the element is moved. 0 if the element is not a member of source and no operation was performed.</returns>
    public Task<bool> MoveMemberAsync(string destination, RedisValue value)
        => RedisDb.SetMoveAsync(KeyName, destination, value);

    /// <summary>
    /// Removes and returns a random element from the set value stored at key.
    /// </summary>
    /// <returns>The removed element, or nil when key does not exist.</returns>
    public Task<RedisValue> PopAsync()
        => RedisDb.SetPopAsync(KeyName);

    /// <summary>
    /// Removes and returns the specified number of random elements from the set value stored at key.
    /// </summary>
    /// <param name="count">The number of elements to return.</param>
    /// <returns>An array of elements, or an empty array when key does not exist.</returns>
    public Task<RedisValue[]> PopAsync(long count)
        => RedisDb.SetPopAsync(KeyName, count);

    /// <summary>
    /// Return a random element from the set value stored at key.
    /// </summary>
    /// <returns>The randomly selected element, or nil when key does not exist</returns>
    public Task<RedisValue> RandomMemberAsync()
        => RedisDb.SetRandomMemberAsync(KeyName);

    /// <summary>
    /// Return an array of count distinct elements if count is positive.
    /// If called with a negative count the behavior changes and the command is allowed to return the same element multiple times.
    /// </summary>
    /// <param name="count">The count of members to get.</param>
    /// <returns>An array of elements, or an empty array when key does not exist</returns>
    public Task<RedisValue[]> RandomMembersAsync(long count)
        => RedisDb.SetRandomMembersAsync(KeyName, count);

    /// <summary>
    /// Remove the specified member from the set stored at key.
    /// Specified members that are not a member of this set are ignored.
    /// </summary>
    /// <param name="value">The value to remove.</param>
    /// <returns>True if the specified member was already present in the set, else False</returns>
    public Task<bool> RemoveMemberAsync(RedisValue value)
        => RedisDb.SetRemoveAsync(KeyName, value);

    /// <summary>
    /// Remove the specified members from the set stored at key.
    /// Specified members that are not a member of this set are ignored.
    /// </summary>
    /// <param name="values">The values to remove.</param>
    /// <returns>The number of members that were removed from the set, not including non-existing members.</returns>
    public Task<long> RemoveMembersAsync(RedisValue[] values)
        => RedisDb.SetRemoveAsync(KeyName, values);

    /// <summary>
    /// Incrementally iterate over a set.
    /// </summary>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="batchSize">The page size to iterate by.</param>
    /// <returns>Yields all matching elements of the set.</returns>
    public IAsyncEnumerable<RedisValue> ScanAsync(RedisValue pattern, int batchSize = 250)
    => RedisDb.SetScanAsync(KeyName, pattern, batchSize);
}
