using System;
using System.Threading.Tasks;
using MyLab.Redis.Connection;
using MyLab.Redis.Values;

namespace MyLab.Redis.CommandModel
{
    /// <summary>
    /// Represent a command with result
    /// </summary>
    public abstract class FuncRedisCommand<TResp, TRedisResp> : RedisCommand, IFuncRedisCommand<TResp>
        where TRedisResp : IRedisValue
    {
        /// <summary>
        /// Initializes a new instance of <see cref="FuncRedisCommand{TResp,TRedisResp}"/>
        /// </summary>
        protected FuncRedisCommand(string name) : base(name)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FuncRedisCommand{TResp,TRedisResp}"/>
        /// </summary>
        protected FuncRedisCommand(string name, string key) : base(name, key)
        {
        }

        /// <summary>
        /// Override to convert response
        /// </summary>
        protected abstract TResp ConvertResponse(TRedisResp responseValue);

        /// <summary>
        /// Performs command
        /// </summary>
        public async Task<TResp> PerformAsync(IRedisConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            
            var response = await connection.PerformCommandAsync(ToRedisArray());
            
            switch (response)
            {
                case ErrorRedisValue err:
                    throw err.ToException();
                case TRedisResp bizResponse:
                    return ConvertResponse(bizResponse);
                default:
                    throw new InvalidOperationException("Unexpected response type: " + response.GetType().FullName);
            }
        }
    }
}