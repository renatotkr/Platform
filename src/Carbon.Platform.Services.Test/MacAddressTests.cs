using Xunit;

using System.Net.NetworkInformation;
using Carbon.Net;

namespace Carbon.Platform.Tests
{
    public class MacAddressTests
    {
        [Fact]
        public void ParsePhysicalAddress()
        {
            var mac = "0a:7f:32:34:91:94";

            var addr = mac.Replace(':', '-').ToUpper();

            var r1 = PhysicalAddress.Parse(addr);
            var r2 = MacAddress.Parse(mac);
            var r3 = MacAddress.Parse(mac.ToUpper());

            Assert.Equal(6, r1.GetAddressBytes().Length);

            Assert.Equal(6, r2.GetAddressBytes().Length);

            Assert.Equal(r1.GetAddressBytes(), r2.GetAddressBytes());
            Assert.Equal(r1.GetAddressBytes(), r3.GetAddressBytes());

            Assert.Equal(mac, r2.ToString().ToLower());
        }

        [Fact]
        public void ParsePhysicalAddress2()
        {
            var mac = "00-0D-3A-10-F1-29";

            var r1 = PhysicalAddress.Parse(mac);
            var r2 = MacAddress.Parse(mac);

            Assert.Equal(6, r1.GetAddressBytes().Length);

            Assert.Equal(6, r2.GetAddressBytes().Length);

            Assert.Equal(r1.GetAddressBytes(), r2.GetAddressBytes());

            Assert.Equal("00:0d:3a:10:f1:29", r2.ToString().ToLower());
        }
        
    }
}
