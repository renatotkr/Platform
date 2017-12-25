using System.Net;

using Xunit;

namespace Carbon.Platform.Computing.Tests
{
    public class HostTests
    {
        [Fact]
        public void Ids()
        {
            Assert.Equal(1L, HostId.Create(0, 1));
        }

        [Fact]
        public void PublicAndPrivateAddressTests()
        {
            var host = GetWithAddresses("192.168.1.1", "54.19.13.1");

            Assert.Equal(IPAddress.Parse("192.168.1.1"), host.PrivateIp);
            Assert.Equal(IPAddress.Parse("54.19.13.1"),  host.PublicIp);
        }

        [Fact]
        public void PrivateAddressOnlyTests()
        {
            var host = GetWithAddresses("192.168.1.1");

            Assert.Equal(IPAddress.Parse("192.168.1.1"), host.PrivateIp);
            Assert.Null(host.PublicIp);
        }

        [Fact]
        public void NoAddressesTest()
        {
            var host = GetWithAddresses();

            Assert.Null(host.PrivateIp);
            Assert.Null(host.PublicIp);
        }

        private HostInfo GetWithAddresses(params string[] addresses)
        {
            return new HostInfo(
               id            : 1,
               addresses     : addresses,
               environmentId : 1,
               clusterId     : 1,
               machineTypeId : 1,
               ownerId       : 1,
               imageId       : 1,
               locationId    : 1,
               resource      : default
           );
        }
    }
}