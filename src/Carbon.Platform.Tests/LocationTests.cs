using Xunit;

namespace Carbon.Platform.Tests
{
    public class CloudLocationTests
    {
        [Fact]
        public void AllZones()
        {
            foreach (var letter in new[] { 'a', 'b', 'c', 'd', 'e' })
            {
                var location = Locations.Get(ResourceProvider.Amazon, "us-east-1" + letter);

                Assert.Equal(letter.ToString(), location.ZoneName);
            }
        }

        [Fact]
        public void AwsRegions1()
        {
            var location = Locations.Get(ResourceProvider.Amazon, "us-east-1");
            
            Assert.Equal("us-east-1", location.Name);
            Assert.Equal("us-east-1", location.RegionName);
            Assert.Equal(ResourceProvider.Amazon, location.Provider);
            Assert.Equal(ResourceType.Region, location.Type);
        }

        [Fact]
        public void AwsRegion1WithZone()
        {
            var location = Locations.Get(ResourceProvider.Amazon, "us-east-1a");

            Assert.Equal("us-east-1", location.RegionName);
            Assert.Equal("us-east-1a", location.Name);
            Assert.Equal("a", location.ZoneName);
            Assert.Equal(ResourceProvider.Amazon, location.Provider);
            Assert.Equal(ResourceType.Zone, location.Type);
        }

        [Fact]
        public void LocationRange()
        {
            var s = LocationId.Create(ResourceProvider.Amazon, 1, 000);
            var e = LocationId.Create(ResourceProvider.Amazon, 1, 255);

            Assert.Equal(4295032832, s.Value);
            Assert.Equal(4295033087, e.Value);
        }

        [Fact]
        public void GetTests()
        {
            Assert.Equal(Locations.Get(Locations.Google_Asia.Id), Locations.Google_Asia);
            Assert.Equal(Locations.Get(Locations.Google_EU.Id),   Locations.Google_EU);
            Assert.Equal(Locations.Get(Locations.Google_US.Id),   Locations.Google_US);

            Assert.Equal(Locations.Get(Locations.Amazon_US_East1.Id), Locations.Amazon_US_East1);
            Assert.Equal(Locations.Get(Locations.Amazon_US_West1.Id), Locations.Amazon_US_West1);
            Assert.Equal(Locations.Get(Locations.Amazon_US_West2.Id), Locations.Amazon_US_West2);

            Assert.Equal(Locations.Get(Locations.Amazon_AP_NorthEast1.Id), Locations.Amazon_AP_NorthEast1);
        }

        [Fact]
        public void ZoneIds()
        {
            var letters = new [] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

            for (var i = 0; i < letters.Length; i++)
            {
                var letter = letters[i];

                Assert.Equal(i + 1, LocationHelper.GetZoneNumber(letter));
                Assert.Equal(i + 1, LocationHelper.GetZoneNumber(char.ToLower(letter)));
            }
        }

        [Fact]
        public void TestProperties()
        {
            var region = new LocationInfo {
                ProviderId = ResourceProvider.Amazon.Id,
                Name = "us-east-1"
            };

            Assert.Equal("aws",                   region.Provider.Code);
            Assert.Equal(1,                       region.Provider.Id);
            Assert.Equal("us-east-1",             region.Name);
            Assert.Equal(ResourceProvider.Amazon, region.Provider);
        }

        [Fact]
        public void MultiRegionalTests()
        {
            Assert.True(Locations.Google_Asia.IsMultiRegional);
            Assert.True(Locations.Google_EU.IsMultiRegional);
            Assert.True(Locations.Google_US.IsMultiRegional);

            Assert.False(Locations.Amazon_AP_NorthEast1.IsMultiRegional);

        }

        [Fact]
        public void LocationEquality()
        {
            var a1 = new LocationInfo { Id = 1, ProviderId = 1, Name = "a" };
            var a2 = new LocationInfo { Id = 1, ProviderId = 1, Name = "a" };
            var b1 = new LocationInfo { Id = 1, ProviderId = 1, Name = "b" };

            Assert.True(a1.Equals(a2));
            Assert.False(a1.Equals(b1));
        }

        [Fact]
        public void LocationIdTest()
        {
            var ab = LocationId.Create(4295032833); // Amazon_USEast1_A

            Assert.Equal(1,          ab.ProviderId);    // Amazon
            Assert.Equal(1,          ab.RegionNumber);  // 1
            Assert.Equal(1,          ab.ZoneNumber);    // A
            Assert.Equal(4295032833, ab.Value);

            Assert.Equal(4295032833, Locations.Amazon_US_East1.WithZone('A').Id);

            var zone = LocationInfo.FromId(ab.Value, "us-east-1a");

            Assert.Equal(ResourceProvider.Amazon, zone.Provider);
            Assert.Equal(ResourceType.Zone, zone.Type);
        }

        [Fact]
        public void WithZoneTests()
        {
            var a = Locations.Amazon_US_East1.WithZone('A');

            Assert.Equal(1, a.ProviderId);
            Assert.Equal(4295032833, a.Id);
            Assert.Equal("us-east-1a", a.Name);

            Assert.Equal("asia-east1-c", Locations.Google_AsiaEast1.WithZone('C').Name);
        }

        [Fact]
        public void Ids()
        {
            Assert.Equal(4295032832, Locations.Amazon_US_East1.Id);
            Assert.Equal(4295032833, Locations.Amazon_US_East1.WithZone('A').Id);
            Assert.Equal(4295032834, Locations.Amazon_US_East1.WithZone('B').Id);
        }
    }
}