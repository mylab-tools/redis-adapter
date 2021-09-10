using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyLab.Redis.Services
{
    public interface IRedisConnectionProvider
    {
        IConnectionMultiplexer Provide();
    }
}
