using Xunit;

namespace Carbon.Hosting.Tests
{
    public class BindingInfoTests
    {
        [Fact]
        public void Parse1()
        {
            var info = SiteBindingInfo.Parse("*:80:");

            Assert.Equal("*", info.Ip);
            Assert.Equal(80,  info.Port);
            Assert.Equal("",  info.HostName);
        }
    }
}
