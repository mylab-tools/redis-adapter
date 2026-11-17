using StackExchange.Redis;

namespace MyLab.Redis.Locking.Redlock;

record RedlockAcquireResult(RedisKey Key, String Token, TimeSpan Validity);