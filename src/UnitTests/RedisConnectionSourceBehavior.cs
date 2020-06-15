using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyLab.Redis.Connection;
using Xunit;

namespace UnitTests
{
    public class RedisConnectionSourceBehavior
    {
        [Fact]
        public void ShouldProvideConnectionExclusive()
        {
            //Arrange
            var src = new RedisConnectionSource(
                new TestTcpClientProvider(), 
                new DefaultRedisConnectionFactory(Encoding.UTF8), 
                TimeSpan.FromMilliseconds(1000),
                15, null);

            bool task1HasConnection= false;
            bool task2HasConnection = false;

            bool hasCollision = false;

            Func<Task> task1Action = async () =>
            {
                using (var c = await src.ProvideConnectionAsync())
                {
                    task1HasConnection = true;

                    if (task2HasConnection)
                        hasCollision = true;
                    else 
                        Thread.Sleep(100);
                }

                task1HasConnection = false;
            };

            Func<Task> task2Action = async () =>
            {
                using (var c = await src.ProvideConnectionAsync())
                {
                    task2HasConnection = true;

                    if (task1HasConnection)
                        hasCollision = true;
                    else
                        Thread.Sleep(100);
                }

                task2HasConnection = false;
            };

            //Act

            var task1 = Task.Run(task1Action);
            var task2 = Task.Run(task2Action);
            Task.WaitAll(task1, task2);

            //Assert
            Assert.False(task1.IsFaulted);
            Assert.False(task2.IsFaulted);
            Assert.False(hasCollision);
        }

        [Fact]
        public void ShouldThrowExceptionWhenConnectionRequestTimeout()
        {
            //Arrange
            var src = new RedisConnectionSource(
                new TestTcpClientProvider(), 
                new DefaultRedisConnectionFactory(Encoding.UTF8), 
                TimeSpan.FromMilliseconds(100),
                15, null);

            Exception task2Exception = null;

            Func<Task> task1Action = async () =>
            {
                using (var c = await src.ProvideConnectionAsync())
                {
                    Thread.Sleep(200);
                }
            };

            Func<Task> task2Action = async () =>
            {
                try
                {
                    using (var c = await src.ProvideConnectionAsync())
                    {
                    }
                }
                catch (Exception e)
                {
                    task2Exception = e;
                }
            };

            //Act

            var task1 = Task.Run(task1Action);
            var task2 = Task.Run(task2Action);
            Task.WaitAll(task1, task2);

            //Assert
            Assert.False(task1.IsFaulted);
            Assert.False(task2.IsFaulted);
            Assert.NotNull(task2Exception);
            Assert.IsType<ConnectionRequestTimeoutException>(task2Exception);

        }

        class TestTcpClientProvider : ITcpClientProvider
        {
            public TcpClient Provide(out bool isNew)
            {
                isNew = false;
                return new TcpClient();
            }

            public void Dispose()
            {
            }
        }
    }
}
