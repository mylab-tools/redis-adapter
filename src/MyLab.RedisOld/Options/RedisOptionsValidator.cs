using System.Linq;
using Microsoft.Extensions.Options;
using System.Text;

namespace MyLab.Redis.Options
{
    internal class RedisOptionsValidator : IValidateOptions<RedisOptions>
    {
        public ValidateOptionsResult Validate(string? name, RedisOptions options)
        {
            StringBuilder? failure = null;

            if (options.ConnectionString == null)
            {
                (failure ??= new()).AppendLine("Connection string is not defined");
            }
            
            if (options.Caching?.Caches != null && options.Caching.Caches.Any(c => c.Name == null))
            {
                (failure ??= new()).AppendLine("Cache option must have a name");
            }

            if (options.Locking?.Locks != null && options.Locking.Locks.Any(c => c.Name == null))
            {
                (failure ??= new()).AppendLine("Lock option must have a name");
            }

            return failure is not null
                ? ValidateOptionsResult.Fail(failure.ToString())
                : ValidateOptionsResult.Success;
        }
    }
}
