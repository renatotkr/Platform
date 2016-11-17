using Xunit;

namespace Carbon.Computing.Tests
{
    public class NetworkPortTests
    {
        [Fact]
        public void Parse1()
        {
            Assert.Equal(80, NetworkPort.Parse("80").Start);
            Assert.Equal(80, NetworkPort.Parse("80/tcp").Start);

            Assert.Equal(NetworkProtocal.TCP,   NetworkPort.Parse("80/tcp").Protocal);
            Assert.Equal(NetworkProtocal.HTTP,  NetworkPort.Parse("80/http").Protocal);
            Assert.Equal(NetworkProtocal.HTTPS, NetworkPort.Parse("443/https").Protocal);

            Assert.Equal("80", NetworkPort.Parse("80").ToString());

            Assert.Equal("81/http", NetworkPort.Parse("81/http").ToString());

            Assert.Equal("http", NetworkPort.Parse("80/http").ToString());
            Assert.Equal("https", NetworkPort.Parse("443/https").ToString());

            Assert.Equal(80, NetworkPort.Parse("http").Start);
            Assert.Equal(443, NetworkPort.Parse("https").Start);

        }

        /*
        
        [Fact]
        public void ParseList()
        {
            var list = NetworkPortList.Parse("80/tcp,81/http");

            Assert.Equal(2, list.Count);

            Assert.Equal(80, list[0].Start);
            Assert.Equal(NetworkProtocal.TCP, list[0].Protocal);
            Assert.Equal(81, list[1].Start);
            Assert.Equal(NetworkProtocal.HTTP, list[1].Protocal);


        }
        */
    }
}
