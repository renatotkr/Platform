using Xunit;

namespace Carbon.Platform.Resources
{
	public class ResourceTests
	{
        [Fact]
        public void ParseHost()
        {
            var resource = ManagedResource.Parse("aws:host/18");

            Assert.Equal("AWS",             ResourceProvider.Get(resource.ProviderId).Name);
            Assert.Equal(ResourceType.Host, resource.Type);
            Assert.Equal("18",              resource.ResourceId);
            Assert.Equal("aws:host/18",     resource.ToString());
        }

        [Fact]
        public void ParseHostWithZone()
        {
            var resource = ManagedResource.Parse("aws:us-east-1:host/18");

            Assert.Equal(2,                 resource.ProviderId);
            Assert.Equal(ResourceType.Host, resource.Type);
            Assert.Equal("18",              resource.ResourceId);

            Assert.Equal("aws:us-east-1:host/18", resource.ToString());
        }

        [Fact]
        public void ParseLocation()
        {
            var resource = ManagedResource.Parse("aws:location/us-east-1b");

            Assert.Equal(2,                     resource.ProviderId);
            Assert.Equal(ResourceType.Location, resource.Type);
            Assert.Equal("us-east-1b",          resource.ResourceId);

            Assert.Equal("aws:location/us-east-1b", resource.ToString());
        }

        
        [Theory]
        [InlineData(ResourceType.Host,             "host")]
        [InlineData(ResourceType.MachineImage,     "machineImage")]
        [InlineData(ResourceType.Program,          "program")]
        public void ComputingTypes(ResourceType type, string name)
        {
            Assert.Equal(name, type.GetName());
            Assert.Equal(type, ResourceTypeHelper.Parse(name));
        }
        [Theory]
        [InlineData(ResourceType.Bucket,           "bucket")]
        [InlineData(ResourceType.Database,         "database")]
        [InlineData(ResourceType.Domain,           "domain")]
        [InlineData(ResourceType.Repository,       "repository")]
        public void Types(ResourceType type, string name)
        {
            Assert.Equal(name, type.GetName());
            Assert.Equal(type, ResourceTypeHelper.Parse(name));
        }

        [Theory]
        [InlineData(ResourceType.Network,           "network")]
        [InlineData(ResourceType.NetworkInterface,  "networkInterface")]
        [InlineData(ResourceType.Subnet,            "subnet")]
        public void NetworkingTypes(ResourceType type, string name)
        {
            Assert.Equal(name, type.GetName());
            Assert.Equal(type, ResourceTypeHelper.Parse(name));
        }
    }
}