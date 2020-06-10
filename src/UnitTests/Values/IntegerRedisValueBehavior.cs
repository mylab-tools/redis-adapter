using System.IO;
using System.Text;
using System.Threading.Tasks;
using MyLab.Redis.Values;
using Xunit;

namespace UnitTests.Values
{
    public class IntegerRedisValueBehavior
    {
        [Fact]
        public async Task ShouldSerialize()
        {
            //Arrange
            var v = new IntegerRedisValue(1000);

            //Act
            var serialized = await v.SerializeAsync();

            //Assert
            Assert.Equal("1000", serialized);
        }

        [Fact]
        public async Task ShouldRead()
        {
            //Arrange
            var data = Encoding.UTF8.GetBytes(":100\r\n");
            IntegerRedisValue val;
            
            //Act
            await using (var mem = new MemoryStream(data))
            {
                var valRdr = new ValuesStreamReader(mem){Encoding = Encoding.UTF8};
                val = (IntegerRedisValue) await valRdr.ReadValueAsync();
            }

            //Assert
            Assert.Equal(100, val.Value);
        }
    }
    
}
