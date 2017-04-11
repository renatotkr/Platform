using Xunit;

namespace Carbon.Platform.Apps.Tests
{
    /*
    public class ApplicationNameTests
    {
        // forbidden characters in IIS
        static readonly char[] forbiddenChars = {
            '&', '/', '\\', ':', '*', '?', '|', '<', '>', '[', ']', '+', '=', ';', ',', '@'
        };

        [Fact]
        public void Validate()
        {
            Assert.True(ApplicationName.Validate("carbon"));
            Assert.True(ApplicationName.Validate("carbon_a"));
            Assert.True(ApplicationName.Validate("carbon_b"));

            foreach (var c in forbiddenChars)
            {
                Assert.False(ApplicationName.Validate("app" + c));    
            }

            // 65 characters
            Assert.False(ApplicationName.Validate("carboncarboncarboncarboncarboncarboncarboncarboncarboncarboncarboncarboncarbon"));

        }

    }
    */
}
