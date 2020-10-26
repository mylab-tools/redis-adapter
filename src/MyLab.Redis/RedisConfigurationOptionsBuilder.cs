using StackExchange.Redis;

namespace MyLab.Redis
{
    class RedisConfigurationOptionsBuilder
    {
        private readonly RedisOptions _options;

        public RedisConfigurationOptionsBuilder(RedisOptions options)
        {
            _options = options;
        }

        public ConfigurationOptions Build()
        {
            var c = ConfigurationOptions.Parse(_options.ConnectionString);
            if (!string.IsNullOrEmpty(_options.Password))
                c.Password = _options.Password;

            return c;
        }
    }
}