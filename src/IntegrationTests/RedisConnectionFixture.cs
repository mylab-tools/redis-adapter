using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyLab.Redis.Commands.Keys;
using MyLab.Redis.Connection;
using MyLab.Redis.Values;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class RedisConnectionFixture : IAsyncLifetime
    {
        private readonly TestRedisConnectionFactory _connFactory;
        private ITestOutputHelper _output;

        private readonly List<string> _usedKeys = new List<string>();

        public RedisConnectionSource Source { get; }

        public ITestOutputHelper Output
        {
            get => _output;
            set
            {
                _output = value;
                _connFactory.Output = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RedisConnectionFixture"/>
        /// </summary>
        public RedisConnectionFixture()
        {
            var host = Environment.GetEnvironmentVariable("MYLAB_TEST_REDIS");

            if(string.IsNullOrEmpty(host))
                throw new InvalidOperationException("Environment variable 'MYLAB_TEST_REDIS' not defined");


            _connFactory = new TestRedisConnectionFactory();


            Source = new RedisConnectionSource(
                new DefaultTcpClientProvider(host, 6379),
                _connFactory,
                TimeSpan.FromSeconds(5),
                15, null);
        }

        public string NewKey(MethodBase method)
        {
            var key = TestKey.New(method);

            _usedKeys.Add(key);

            return key;
        }

        public string NewKeyForTheory(MethodBase method)
        {
            var key = TestKey.NewForTheory(method);

            _usedKeys.Add(key);

            return key;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            Output = null;

            if (_usedKeys.Count > 0)
            {
                var delKey = new DeleteKeyRedisCmd(_usedKeys);
                await delKey.PerformAsync(await Source.ProvideConnectionAsync());
            }

            Source?.Dispose();
        }
    }

    internal class TestRedisConnectionFactory : IRedisConnectionFactory
    {
        public ITestOutputHelper Output { get; set; }

        public IRedisConnection Create(TcpClient client, IDisposable syncDisposer)
        {
            return new TestRedisConnection(client, syncDisposer, () => Output);
        }

        class TestRedisConnection : IRedisConnection
        {
            private readonly Func<ITestOutputHelper> _outputProvider;
            private readonly DefaultRedisConnection _inner;

            public TestRedisConnection(TcpClient client, IDisposable syncDisposer, Func<ITestOutputHelper> outputProvider)
            {
                _outputProvider = outputProvider;
                _inner = new DefaultRedisConnection(client, syncDisposer);
            }

            public void Dispose()
            {
                _inner.Dispose();
            }

            public async Task<IRedisValue> PerformCommandAsync(ArrayRedisValue command)
            {
                var resp = await _inner.PerformCommandAsync(command);
                
                if(!(command.Items.FirstOrDefault() is BulkStringRedisValue strVal && strVal.Value == "SELECT"))
                    await Report(command, resp);

                return resp;
            }

            async Task Report(IRedisValue write, IRedisValue read)
            {
                var output = _outputProvider();

                if (output == null)
                    return;

                string writeVal = null, readVal = null;

                if (write != null)
                {
                    await using var stream = new MemoryStream();
                    using var reader = new StreamReader(stream);
                    await write.WriteAsync(stream, Encoding.UTF8);
                    stream.Position = 0;
                    writeVal = reader.ReadToEnd();
                }

                if (read != null)
                {
                    await using var stream = new MemoryStream();
                    using var reader = new StreamReader(stream);
                    await read.WriteAsync(stream, Encoding.UTF8);
                    stream.Position = 0;
                    readVal = reader.ReadToEnd();
                }

                output.WriteLine("REQUEST: ");
                output.WriteLine("");
                output.WriteLine(writeVal);
                output.WriteLine("");
                output.WriteLine("RESPONSE: ");
                output.WriteLine("");
                output.WriteLine(readVal);
                output.WriteLine("");
            }
        }
    }
}
