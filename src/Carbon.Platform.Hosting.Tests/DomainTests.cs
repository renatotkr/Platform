using Xunit;

namespace Carbon.Platform.Hosting.Tests
{
    public class DomainTests
    {
        [Fact]
		public void A()
		{
            var domain = new Domain(
                id: 1,
                name: "processor.ai",
                ownerId: 1, 
                nameServers: new[] {
                    "1.dns.host",
                    "2.dns.host"
                },
                flags: DomainFlags.Managed
            );

            Assert.Equal(1,                   domain.Id);
            Assert.Equal("ai/processor",      domain.Path);
            Assert.Equal("processor.ai",      domain.Name);
            Assert.Equal(1,                   domain.OwnerId);
            Assert.Equal(2,                   domain.NameServers.Length);
            Assert.Equal(DomainFlags.Managed, domain.Flags);
        }     
    }
}