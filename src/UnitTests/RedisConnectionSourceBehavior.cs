using System;
using System.Net.Sockets;
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
            var src = new RedisConnectionSource(new TestTcpClientProvider(), TimeSpan.FromMilliseconds(1000));

            bool task1HasConnection= false;
            bool task2HasConnection = false;

            bool hasCollision = false;

            Action task1Action = () =>
            {
                using (var c = src.ProvideConnection())
                {
                    task1HasConnection = true;

                    if (task2HasConnection)
                        hasCollision = true;
                    else 
                        Thread.Sleep(100);
                }

                task1HasConnection = false;
            };

            Action task2Action = () =>
            {
                using (var c = src.ProvideConnection())
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
            var src = new RedisConnectionSource(new TestTcpClientProvider(), TimeSpan.FromMilliseconds(100));
            Exception task2Exception = null;

            Action task1Action = () =>
            {
                using (var c = src.ProvideConnection())
                {
                    Thread.Sleep(200);
                }
            };

            Action task2Action = () =>
            {
                try
                {
                    using (var c = src.ProvideConnection())
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
            public TcpClient Provide()
            {
                return new TcpClient();
            }
        }
    }
}
