using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyLab.Redis.Connection;

namespace MyLab.Redis.CommandModel
{
    /// <summary>
    /// Defines Redis command with result
    /// </summary>
    public interface IFuncRedisCommand<TResp>
    {
        /// <summary>
        /// Performs command
        /// </summary>
        Task<TResp> PerformAsync(IRedisConnection connection);
    }
}
