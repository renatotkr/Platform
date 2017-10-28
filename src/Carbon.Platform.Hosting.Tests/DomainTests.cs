using Carbon.Json;
using Carbon.Net.Dns;
using Xunit;

namespace Carbon.Platform.Hosting.Tests
{
    public class DomainTests
    {
        [Fact]
		public void A()
		{
            var domain = new Domain(
                id      : 1,
                name    : "processor.ai",
                ownerId : 1, 
                flags   : DomainFlags.Managed | DomainFlags.Authoritative
            );

            var flags = DomainFlags.Managed | DomainFlags.Authoritative;

            Assert.Equal(1,                   domain.Id);
            Assert.Equal("ai/processor",      domain.Path);
            Assert.Equal("processor.ai",      domain.Name);
            Assert.Equal(1,                   domain.OwnerId);
            Assert.Equal(flags,               domain.Flags);
        }

        [Fact]
        public void DeserializeCreateDomainRequestFromJson()
        {
            var request = JsonObject.Parse(
                @"{ ""name"": ""www.processor.ai"", ""environmentId"": 5, ""ownerId"": 1 }"
            ).As<CreateDomainRequest>();

        
            Assert.Equal("www.processor.ai", request.Name);
            Assert.Equal(1, request.OwnerId);
            Assert.Equal(5, request.EnvironmentId);
        }

        [Fact]
        public void DeserializeCreateDomainRecordRequestFromJson()
        {
            var request = JsonObject.Parse(
                @"{ ""name"": ""www.processor.ai"", type: ""CNAME"", ""domainId"": 1 }"
            ).As<CreateDomainRecordRequest>();
            
            Assert.Equal("www.processor.ai",    request.Name);
            Assert.Equal(DnsRecordType.CNAME,   request.Type);
            Assert.Equal(1,                     request.DomainId);
        }
    }
}