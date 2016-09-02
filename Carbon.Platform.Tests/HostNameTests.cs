using Xunit;

namespace Carbon.Hosting.Tests
{
    public class HostNameTests
    {
        [Fact]
        public void IsValidHostName()
        {
            Assert.True(Hostname.IsValid("abc"));
            Assert.True(Hostname.IsValid("abc.com"));

            //Assert.False(HostName.IsValid("❄"));
            Assert.False(Hostname.IsValid("/"));
            Assert.False(Hostname.IsValid("oranges/"));
            Assert.False(Hostname.IsValid("oranges\\"));
        }
    }
}
