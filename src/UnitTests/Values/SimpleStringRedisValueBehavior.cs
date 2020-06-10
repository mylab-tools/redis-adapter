using System.IO;
using System.Text;
using System.Threading.Tasks;
using MyLab.Redis.Values;
using Xunit;

namespace UnitTests.Values
{
    public class SimpleStringRedisValueBehavior
    {
        [Fact]
        public async Task ShouldSerialize()
        {
            //Arrange
            var v = new StringRedisValue("foo");

            //Act
            var serialized = await v.SerializeAsync();

            //Assert
            Assert.Equal("foo", serialized);
        }

        [Fact]
        public async Task ShouldParse()
        {
            //Arrange
            var data = Encoding.UTF8.GetBytes("+foo\r\n");

            //Act
            StringRedisValue val;

            await using (var mem = new MemoryStream(data))
            {
                var valRdr = new ValuesStreamReader(mem){Encoding = Encoding.UTF8};
                val = (StringRedisValue)await valRdr.ReadValueAsync();
            }
                

            //Assert
            Assert.Equal("foo", val.Value);
        }
    }
}
