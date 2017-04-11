using Xunit;

namespace Carbon.Net.Tests
{
    public class NetworkPortTests
    {
        [Fact]
        public void Parse1()
        {
            Assert.Equal(80, Listener.Parse("80").Port);
            // Assert.Equal(80, Listener.Parse("80/tcp").Port);

            // Assert.Equal(ApplicationProtocal.TCP,   Listener.Parse("80/tcp").Protocal);
            Assert.Equal(ApplicationProtocal.HTTP,  Listener.Parse("80/http").Protocal);
            Assert.Equal(ApplicationProtocal.HTTPS, Listener.Parse("443/https").Protocal);

            Assert.Equal("80", Listener.Parse("80").ToString());

            Assert.Equal("81/http", Listener.Parse("81/http").ToString());

            Assert.Equal("http", Listener.Parse("80/http").ToString());
            Assert.Equal("https", Listener.Parse("443/https").ToString());

            Assert.Equal(80, Listener.Parse("http").Port);
            Assert.Equal(443, Listener.Parse("https").Port);
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
