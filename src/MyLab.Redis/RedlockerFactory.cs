using System;
using System.Linq;
using MyLab.Redis.ObjectModel;

namespace MyLab.Redis
{
    /// <summary>
    /// Creates Redlock
    /// </summary>
    public class RedlockerFactory
    {
        private readonly RedisDbLink _database;
        private readonly RedisOptions _options;

        /// <summary>
        /// Initializes a new instance of <see cref="RedlockerFactory"/>
        /// </summary>
        public RedlockerFactory(RedisDbLink database, RedisOptions options)
        {
            _database = database;
            _options = options;
        }

        /// <summary>
        /// Creates a lock
        /// </summary>
        public Redlocker Create(string name, string childId = null)
        {
            var opt = _options.Locking?.Locks?.FirstOrDefault(o => o.Name == name);
            if (opt == null)
                throw new InvalidOperationException($"Redlock '{name}' not found");

            var keyNameBuilder = new KeyNameBuilder(name)
            {
                Prefix = _options.Locking.KeyPrefix,
                Suffix = childId
            };

            var lockKeyName = keyNameBuilder.Build();
            var rLocker =  new Redlocker(_database.Provider, lockKeyName);

            if (!string.IsNullOrEmpty(opt.Expiry))
                rLocker.Expiry = OptionsExpiryParser.Parse(opt.Expiry);

            if (!string.IsNullOrEmpty(opt.DefaultTimeout))
                rLocker.DefaultLockingTimeout = OptionsExpiryParser.Parse(opt.DefaultTimeout);

            return rLocker;
        }
    }
}