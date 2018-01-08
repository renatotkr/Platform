using System.Net;
using Xunit;

namespace Carbon.Net.Tests
{
    public class CidrTests
    {
        [Fact]
        public void A()
        {
            var a = Cidr.Parse("92.223.124.5/32");
            
            Assert.Equal(IPAddress.Parse("92.223.124.5"), a.Start);
            Assert.Equal(IPAddress.Parse("92.223.124.5"), a.End);
        }

        [Fact]
        public void B()
        {
            var a = Cidr.Parse("92.223.126.204/32");

            Assert.Equal(32, a.Suffix);
            Assert.Equal(IPAddress.Parse("92.223.126.204"), a.Start);
            Assert.Equal(IPAddress.Parse("92.223.126.204"), a.End);
        }
    }
}
