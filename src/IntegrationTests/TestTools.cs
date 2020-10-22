using MyLab.Redis;

namespace IntegrationTests
{
    static class TestTools
    {
        public static RedisOptions Options => new RedisOptions()
        {
            ConnectionString = "localhost:9110"
        };
    }
}