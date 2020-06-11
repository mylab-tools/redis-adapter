using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLab.Redis.Connection;
using MyLab.Redis.Values;

namespace MyLab.Redis.Commands
{
    /// <summary>
    /// Contains redis command name and parameters
    /// </summary>
    public abstract class RedisCommand

    {
        /// <summary>
        /// Gets a command name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a key name 
        /// </summary>
        /// <remarks>Optional</remarks>
        public string Key { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="RedisCommand"/>
        /// </summary>
        protected RedisCommand(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            Name = name.ToUpper();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RedisCommand"/>
        /// </summary>
        protected RedisCommand(string name, string key)
            : this(name)
        {
            Key = key;
        }

        /// <summary>
        /// Gets command parameters
        /// </summary>
        protected virtual void GetParameters(CommandParameters parameters) { }


        public ArrayRedisValue ToRedisArray()
        {
            var vals = new List<IRedisValue> { new BulkStringRedisValue(Name) };

            if (!string.IsNullOrWhiteSpace(Key))
                vals.Add(new BulkStringRedisValue(Key));

            var p = new CommandParameters();
            GetParameters(p);
            vals.AddRange(p.ToArray().Select(s => new BulkStringRedisValue(s)));

            return new ArrayRedisValue(vals);
        }
    }

    ///// <summary>
    ///// Represent a command without result
    ///// </summary>
    //public class ActionRedisCommand : RedisCommand
    //{
    //    /// <summary>
    //    /// Initializes a new instance of <see cref="ActionRedisCommand"/>
    //    /// </summary>
    //    protected ActionRedisCommand(string name) : base(name)
    //    {
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of <see cref="ActionRedisCommand"/>
    //    /// </summary>
    //    protected ActionRedisCommand(string name, string key) : base(name, key)
    //    {
    //    }

    //    /// <summary>
    //    /// Performs command
    //    /// </summary>
    //    public async Task PerformAsync(IRedisConnection connection)
    //    {
    //        if (connection == null) throw new ArgumentNullException(nameof(connection));

    //        IRedisValue res;
    //        ArrayRedisValue sentValue;

    //        using (var scope = await connection.CreateScopeAsync())
    //        {
    //            sentValue = await ((IRedisCommand)this).SendAsync(scope);
    //            res = await scope.ReadAsync();
    //        }

    //        connection.Debugger?.Report(sentValue, res);

    //        if (res is ErrorRedisValue err)
    //            throw err.ToException();
    //    }
    //}

    ///// <summary>
    ///// Represent a command with result
    ///// </summary>
    //public abstract class FuncRedisCommand<TResp, TRedisResp> : RedisCommand
    //    where TRedisResp : IRedisValue
    //{
    //    /// <summary>
    //    /// Initializes a new instance of <see cref="FuncRedisCommand{TResp,TRedisResp}"/>
    //    /// </summary>
    //    protected FuncRedisCommand(string name) : base(name)
    //    {
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of <see cref="FuncRedisCommand{TResp,TRedisResp}"/>
    //    /// </summary>
    //    protected FuncRedisCommand(string name, string key) : base(name, key)
    //    {
    //    }

    //    /// <summary>
    //    /// Override to convert response
    //    /// </summary>
    //    protected abstract TResp ConvertResponse(TRedisResp responseValue);


    //    /// <summary>
    //    /// Performs command
    //    /// </summary>
    //    public async Task<TResp> PerformAsync(IRedisConnection connection)
    //    {
    //        if (connection == null) throw new ArgumentNullException(nameof(connection));

    //        IRedisValue res;
    //        ArrayRedisValue sentValue;

    //        using (var scope = await connection.CreateScopeAsync())
    //        {
    //            sentValue = await ((IRedisCommand)this).SendAsync(scope);
    //            res = await scope.ReadAsync();
    //        }

    //        connection.Debugger?.Report(sentValue, res);

    //        switch (res)
    //        {
    //            case ErrorRedisValue err:
    //                throw err.ToException();
    //            case TRedisResp rresp:
    //                return ConvertResponse(rresp);
    //            default:
    //                throw new InvalidOperationException("Unexpected response type: " + res.GetType().FullName);
    //        }
    //    }
    //}
}
