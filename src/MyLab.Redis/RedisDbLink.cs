using StackExchange.Redis;

namespace MyLab.Redis
{
    public class RedisDbLink
    {
        public IDatabase Object { get; set; }
        public int Index{ get; set; }
    }
}