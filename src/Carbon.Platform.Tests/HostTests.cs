using Xunit;

namespace Carbon.Platform.Computing.Tests
{
    public class HostTests
    {
        [Fact]
        public void Ids()
        {
            var hostId = new HostId { Sequence = 1 };

            Assert.Equal(1, hostId.Value);
        }

        /*
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
        */
    }
}
