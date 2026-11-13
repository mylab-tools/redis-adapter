using StackExchange.Redis;

namespace MyLab.Redis.Core
{
    public class RedisKeyEnumerable(IAsyncEnumerable<RedisKey> enumerable) : IAsyncEnumerable<RedisKey>
    {
        public IScanningCursor? ScanningCursor { get; } = enumerable as IScanningCursor;

        public IAsyncEnumerator<RedisKey> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return enumerable.GetAsyncEnumerator(cancellationToken);
        }
    }
}
