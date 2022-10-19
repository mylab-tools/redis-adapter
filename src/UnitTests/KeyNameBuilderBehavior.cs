using MyLab.Redis;
using Xunit;

namespace UnitTests
{
    public class KeyNameBuilderBehavior
    {
        [Theory]
        [InlineData("foo", null, null, "foo")]
        [InlineData("foo", "bar", null, "bar:foo")]
        [InlineData("foo", null, "baz", "foo:baz")]
        [InlineData("foo", "bar", "baz", "bar:foo:baz")]
        public void ShouldBuildName(string name, string prefix, string suffix, string expectedResult)
        {
            //Arrange
            var b = new KeyNameBuilder(name)
            {
                Prefix = prefix,
                Suffix = suffix
            };

            //Act
            var keyName = b.Build();

            //Assert
            Assert.Equal(expectedResult, keyName);
        }
    }
}
