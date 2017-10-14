using Carbon.Net.Dns;
using Carbon.Platform.Sequences;
using Xunit;

namespace Carbon.Platform.Hosting.Tests
{
    public class DomainRecordTests
    {
        [Fact]
        public void DomainPathTests()
        {
            Assert.Equal("ai/processor",     DomainName.Parse("processor.ai").Path);
            Assert.Equal("ai/processor/www", DomainName.Parse("www.processor.ai").Path);

            // Level overloads
            Assert.Equal("ai/processor/www", DomainName.Parse("yetanother.another.www.processor.ai").GetPath(level: 3));
            Assert.Equal("ai/processor",     DomainName.Parse("yetanother.another.www.processor.ai").GetPath(level: 2));
            Assert.Equal("ai/processor",     DomainName.Parse("another.www.processor.ai").GetPath(level: 2));
            Assert.Equal("ai/processor",     DomainName.Parse("www.processor.ai").GetPath(level: 2));
            Assert.Equal("ai",               DomainName.Parse("www.processor.ai").GetPath(level: 1));
        }

        [Fact]
        public void B()
        {
            var name = DomainName.Parse("www.processor.ai");

            var record = new DomainRecord(
                id          : ScopedId.Create(456, 1),
                name        : "www",
                path        : name.Path,
                type        : DnsRecordType.A,
                value       : "192.168.1.1",
                ttl         : 600,
                flags       : DomainRecordFlags.None
            );

            Assert.Equal(1912602625,         record.Id);
            Assert.Equal("www",              record.Name);
            Assert.Equal("ai/processor/www", record.Path);
            Assert.Equal(456,                record.DomainId);
            Assert.Equal(1,                 (int)record.Type);
            Assert.Equal("192.168.1.1",      record.Value);
            Assert.Equal(600,                record.Ttl);


        }

        [Fact]
        public void C()
        {
            var record = new DomainRecord(
                id       : 19345,
                name     : "*",
                path     : "ai/processor/*",
                type     : DnsRecordType.CNAME,
                value    : "hosted.accelerator.net",
                ttl      : null,
                flags    : DomainRecordFlags.Alias
            );

            Assert.Equal(19345,                    record.Id);
            Assert.Equal("*",                      record.Name);
            Assert.Equal("ai/processor/*",         record.Path);
            Assert.Equal(5,                        (int)record.Type);
            Assert.Equal("hosted.accelerator.net", record.Value);
            Assert.Equal(DomainRecordFlags.Alias,  record.Flags);
            Assert.Null(record.Ttl);
        }
    }
}

/*
{
    name  : "www.processor.ai",
    type  : "A",
    value : "192.168.1.1" 
}
*/
