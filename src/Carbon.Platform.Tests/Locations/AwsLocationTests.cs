using Xunit;

namespace Carbon.Platform.Tests
{
    using static ResourceProvider;

    public class AwsLocationTests
    {
        [Theory]
        [InlineData("us-east-1", 33554688)]
        [InlineData("us-west-2", 33556224)]
        [InlineData("eu-west-3", 33559040)]
        public void AwsRegionsNames(string name, int id)
        {
            var location = Locations.Get(Aws, name);

            Assert.Equal(id, location.Id);
            Assert.Equal(name, location.Name);
            Assert.Equal(name, location.RegionName);
            Assert.Equal(Aws.Id, location.ProviderId);
            Assert.Equal(LocationType.Region, location.Type);
        }

        [Fact]
        public void AwsRegion1WithZone()
        {
            var location = Locations.Get(Aws, "us-east-1a");

            Assert.Equal("us-east-1", location.RegionName);
            Assert.Equal("us-east-1a", location.Name);
            Assert.Equal("a", location.ZoneName);

            Assert.Equal(Aws.Id, location.ProviderId);
            Assert.Equal(LocationType.Zone, location.Type);
        }

        [Fact]
        public void AwsRegion1WithZone2()
        {
            var location = Locations.Aws.Get("us-east-1a");

            Assert.Equal("us-east-1",  location.RegionName);
            Assert.Equal("us-east-1a", location.Name);
            Assert.Equal("a",          location.ZoneName);
        }
    }
}