using System.IO;
using System.Text;
using System.Threading.Tasks;
using MyLab.Redis.Values;
using Xunit;

namespace UnitTests.Values
{
    public class ArrayRedisValueBehavior
    {
        [Fact]
        public async Task ShouldSerialize()
        {
            //Arrange
            var v = new ArrayRedisValue(new IRedisValue[]
            {
                new IntegerRedisValue(1), 
                new IntegerRedisValue(2), 
                new IntegerRedisValue(3), 
                new IntegerRedisValue(4), 
                new BulkStringRedisValue("foobar") 
            });

            //Act
            var serialized = await v.SerializeAsync();

            //Assert
            Assert.Equal("5\r\n:1\r\n:2\r\n:3\r\n:4\r\n$6\r\nfoobar\r\n", serialized);
        }
        
        [Fact]
        public async Task ShouldSerializeWhenNull()
        {
            //Arrange
            var v = new ArrayRedisValue(null);

            //Act
            var serialized = await v.SerializeAsync();

            //Assert
            Assert.Equal("-1\r\n", serialized);
        }

        [Fact]
        public async Task ShouldRead()
        {
            //Arrange
            var data = Encoding.UTF8.GetBytes("5\r\n:1\r\n:2\r\n:3\r\n:4\r\n$6\r\nfoobar\r\n");
            var reader = ArrayRedisValue.CreateReader();
            ArrayRedisValue arr;
            
            //Act
            await using (var mem = new MemoryStream(data))
                arr = (ArrayRedisValue)await reader.ReadAsync(mem, Encoding.UTF8);

            //Assert
            Assert.Equal(5, arr.Items.Count);
            Assert.IsType<IntegerRedisValue>(arr.Items[0]);
            Assert.Equal(1, ((IntegerRedisValue)arr.Items[0]).Value);
            Assert.IsType<IntegerRedisValue>(arr.Items[1]);
            Assert.Equal(2, ((IntegerRedisValue)arr.Items[1]).Value);
            Assert.IsType<IntegerRedisValue>(arr.Items[2]);
            Assert.Equal(3, ((IntegerRedisValue)arr.Items[2]).Value);
            Assert.IsType<IntegerRedisValue>(arr.Items[3]);
            Assert.Equal(4, ((IntegerRedisValue)arr.Items[3]).Value);
            Assert.IsType<BulkStringRedisValue>(arr.Items[4]);
            Assert.Equal("foobar", ((BulkStringRedisValue)arr.Items[4]).Value);
        }
        
        [Fact]
        public async Task ShouldReadWhenNull()
        {
            //Arrange
            var data = Encoding.UTF8.GetBytes("-1\r\n");
            var reader = ArrayRedisValue.CreateReader();
            ArrayRedisValue arr;
            
            //Act
            await using (var mem = new MemoryStream(data))
                arr = (ArrayRedisValue) await reader.ReadAsync(mem, Encoding.UTF8);

            //Assert
            Assert.Null(arr.Items);
        }
    }
}
