using MyLab.Redis;
using Xunit;

namespace UnitTests
{
    public class OptionsExpiryParserBehavior
    {
        [Fact]
        public void ShouldParseTimeSpan()
        {
            //Arrange
            
            //Act
            var ts = OptionsExpiryParser.Parse("00:01:00");

            //Assert
            Assert.Equal(1, ts.TotalMinutes);
        }

        [Fact]
        public void ShouldParseHours()
        {
            //Arrange

            //Act
            var ts = OptionsExpiryParser.Parse("1");

            //Assert
            Assert.Equal(1, ts.TotalHours);
        }
    }
}
