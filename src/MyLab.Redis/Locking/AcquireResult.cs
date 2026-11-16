using StackExchange.Redis;

namespace MyLab.Redis.Locking;

record AcquireResult(RedisKey Key, String Token, TimeSpan Validity);