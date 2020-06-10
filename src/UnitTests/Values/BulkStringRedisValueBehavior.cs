using System.IO;
using System.Text;
using System.Threading.Tasks;
using MyLab.Redis.Values;
using Xunit;

namespace UnitTests.Values
{
    public class BulkStringRedisValueBehavior
    {
        [Fact]
        public async Task ShouldSerialize()
        {
            //Arrange
            var v = new BulkStringRedisValue("foo\r\nfoo");

            //Act
            var serialized = await v.SerializeAsync();

            //Assert
            Assert.Equal("8\r\nfoo\r\nfoo", serialized);
        }

        [Fact]
        public async Task ShouldRead()
        {
            //Arrange
            var data = Encoding.UTF8.GetBytes("$8\r\nfoo\r\nfoo\r\n");
            BulkStringRedisValue val;
            
            
            //Act
            await using (var mem = new MemoryStream(data))
            {
                var rdr = new ValuesStreamReader(mem){Encoding = Encoding.UTF8};
                val = (BulkStringRedisValue)await rdr.ReadValueAsync();
            }

            //Assert
            Assert.Equal("foo\r\nfoo", val.Value);
        }
        
        [Fact]
        public async Task ShouldSerializeWhenNull()
        {
            //Arrange
            var v = new BulkStringRedisValue(null);

            //Act
            var serialized = await v.SerializeAsync();

            //Assert
            Assert.Equal("-1", serialized);
        }

        [Fact]
        public async Task ShouldReadWhenNull()
        {
            //Arrange
            var data = Encoding.UTF8.GetBytes("$-1\r\n");
            BulkStringRedisValue val;
            
            //Act
            await using (var mem = new MemoryStream(data))
            {
                var rdr = new ValuesStreamReader(mem){Encoding = Encoding.UTF8};
                val = (BulkStringRedisValue) await rdr.ReadValueAsync();
            }

            //Assert
            Assert.Null(val.Value);
        }
    }
    
}
