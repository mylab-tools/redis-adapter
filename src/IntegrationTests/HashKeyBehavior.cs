using System;
using System.Threading.Tasks;
using MyLab.Redis;
using MyLab.Redis.ObjectModel;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class HashKeyBehavior
    {
        private readonly ITestOutputHelper _output;

        public HashKeyBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldAddItem()
        {
            await TestTools.PerformTest(_output, async(redis, testKey) =>
            {
                //Arrange
                var hash = redis.Db().Hash(testKey);

                //Act
                await hash.SetAsync("foo-filed", "foo-value");

                //Assert

            });
        }

        class CacheItem
        {
            public int Id { get; set; }
            public int Value { get; set; }
        }
    }
}
