using Xunit;

namespace Carbon.Platform.Tests
{
    public class HostNameTests
    {
        [Fact]
        public void IsValidHostName()
        {
            Assert.True(HostName.IsValid("abc"));
            Assert.True(HostName.IsValid("abc.com"));

            //Assert.False(HostName.IsValid("❄"));
            Assert.False(HostName.IsValid("/"));
            Assert.False(HostName.IsValid("oranges/"));
            Assert.False(HostName.IsValid("oranges\\"));
        }
    }
}
