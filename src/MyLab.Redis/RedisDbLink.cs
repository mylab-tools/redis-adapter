using StackExchange.Redis;

namespace MyLab.Redis
{
    public class RedisDbLink
    {
        public RedisDbProvider Provider { get; set; }
        public int Index{ get; set; }
    }
}