using MyLab.Redis;
using Xunit;

namespace UnitTests
{
    public class KeyNameToolsBehavior
    {
        [Theory]
        [InlineData(null, "foo", "foo")]
        [InlineData("bar", "foo", "bar:foo")]
        public void ShouldBuildName(string prefix, string name, string expectedResult)
        {
            //Arrange
            

            //Act
            var keyName = KeyNameTools.BuildName(prefix, name);

            //Assert
            Assert.Equal(expectedResult, keyName);
        }
    }
}
