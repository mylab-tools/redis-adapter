using System.IO;
using System.Text;
using System.Threading.Tasks;
using MyLab.Redis.Values;
using Xunit;

namespace UnitTests.Values
{
    public class ErrorRedisValueBehavior
    {
        [Fact]
        public async Task ShouldSerialize()
        {
            //Arrange
            var v = new ErrorRedisValue("Some thing wrong");

            //Act
            var serialized = await v.SerializeAsync();

            //Assert
            Assert.Equal("Some thing wrong", serialized);
        }

        [Fact]
        public async Task ShouldParse()
        {
            //Arrange
            var data = Encoding.UTF8.GetBytes("-Error\r\n");
            ErrorRedisValue err;

            //Act
            await using (var mem = new MemoryStream(data))
            {
                var valRdr = new ValuesStreamReader(mem){ Encoding = Encoding.UTF8 };
                err = (ErrorRedisValue) await valRdr.ReadValueAsync();
            }

            //Assert
            Assert.Equal("Error", err.Message);
        }
    }
    
}
