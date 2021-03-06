﻿using Xunit;

namespace Carbon.Platform.Resources
{
    public class ResourceTests
	{
        [Fact]
        public void ParseUnknown()
        {
            var resource = ManagedResource.Parse("aws/i-1");

            Assert.Equal(2, ResourceProvider.Get(resource.ProviderId).Id);
            Assert.Equal("unknown",         resource.Type.Name);
            Assert.Equal("i-1",             resource.ResourceId);
            Assert.Equal("aws:unknown/i-1", resource.ToString());
        }

        [Fact]
        public void ParseWithZone()
        {
            var resource = ManagedResource.Parse("aws:us-east-1a:host/i-1");

            Assert.Equal(2, ResourceProvider.Get(resource.ProviderId).Id);
            Assert.Equal("host", resource.Type.Name);
            Assert.Equal("i-1", resource.ResourceId);
            Assert.Equal("aws:us-east-1a:host/i-1", resource.ToString());
        }

        [Fact]
        public void ParseHost()
        {
            var resource = ManagedResource.Parse("aws:host/18");

            Assert.Equal("AWS",              ResourceProvider.Get(resource.ProviderId).Name);
            Assert.Equal(ResourceTypes.Host, resource.Type);
            Assert.Equal("18",               resource.ResourceId);
            Assert.Equal("aws:host/18",      resource.ToString());
        }

        [Fact]
        public void ParseHostWithZone()
        {
            var resource = ManagedResource.Parse("aws:us-east-1:host/18");

            Assert.Equal(2,                  resource.ProviderId);
            Assert.Equal(ResourceTypes.Host, resource.Type);
            Assert.Equal("18",               resource.ResourceId);

            Assert.Equal("aws:us-east-1:host/18", resource.ToString());
        }

        [Fact]
        public void ParseLocation()
        {
            var resource = ManagedResource.Parse("aws:location/us-east-1b");

            Assert.Equal(2,                      resource.ProviderId);
            Assert.Equal(ResourceTypes.Location, resource.Type);
            Assert.Equal("us-east-1b",           resource.ResourceId);

            Assert.Equal("aws:location/us-east-1b", resource.ToString());
        }
    }
}