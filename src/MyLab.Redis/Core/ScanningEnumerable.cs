using StackExchange.Redis;

namespace MyLab.Redis.Core
{
    public class ScanningEnumerable<T>(IAsyncEnumerable<T> enumerable) : IAsyncEnumerable<T>
    {
        public IScanningCursor? ScanningCursor { get; } = enumerable as IScanningCursor;

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return enumerable.GetAsyncEnumerator(cancellationToken);
        }
    }
}
