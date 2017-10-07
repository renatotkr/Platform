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

        [Fact]
        public void B()
        {
            var record = new DomainRecord(
                id       : 1,
                domainId : 1,
                name     : "www.processor.ai",
                type     : DomainRecordType.A,
                value    : "192.168.1.1",
                ttl      : 600,
                flags    : DomainRecordFlags.None
            );

            Assert.Equal("www.processor.ai", record.Name);
            Assert.Equal("ai/processor/www", record.Path);
            Assert.Equal(1,                  record.DomainId);
            Assert.Equal(1,                  (int)record.Type);
            Assert.Equal("192.168.1.1",      record.Value);
            Assert.Equal(600,                record.Ttl);
        }
    }
}