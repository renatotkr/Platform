using Xunit;

namespace Carbon.Platform.Tests
{
    public class CloudLocationTests
    {
        [Fact]
        public void BorgZones()
        {
            var one = LocationId.Create(ResourceProvider.Borg, 1);

            Assert.Equal(256, LocationId.Create(ResourceProvider.Borg, 1));
            Assert.Equal(1,   LocationId.Create(ResourceProvider.Borg, 0, 1));

        }

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
            Assert.Equal(ResourceProvider.Amazon.Id, location.ProviderId);
            Assert.Equal(LocationType.Region, location.Type);
        }

        [Fact]
        public void AwsRegion1WithZone()
        {
            var location = Locations.Get(ResourceProvider.Amazon, "us-east-1a");
            
            Assert.Equal("us-east-1", location.RegionName);
            Assert.Equal("us-east-1a", location.Name);
            Assert.Equal("a", location.ZoneName);

            // Assert.Equal(ResourceProvider.Amazon.Id, location.ProviderId);
            Assert.Equal(LocationType.Zone, location.Type);
        }

        [Fact]
        public void LocationRange()
        {
            var s = LocationId.Create(ResourceProvider.Amazon, 1, 000);
            var e = LocationId.Create(ResourceProvider.Amazon, 1, 255);

            Assert.Equal(16777472, s.Value);
            Assert.Equal(16777727, e.Value);
        }

        [Fact]
        public void MultiRegion()
        {
            var eu = Locations.Google_EU;

            var id = LocationId.Create(eu.Id);

            Assert.Equal(2, id.ProviderId);
            Assert.Equal(0, id.RegionNumber);
            Assert.Equal(2, id.ZoneNumber);
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

        /*
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
        */

        [Fact]
        public void TestProperties()
        {
            var region = new LocationInfo(Locations.Amazon_US_East1.Id, "us-east-1");

            Assert.Equal("aws",                   region.Provider.Code);
            Assert.Equal(1,                       region.Provider.Id);
            Assert.Equal("us-east-1",             region.Name);
            Assert.Equal(ResourceProvider.Amazon, region.Provider);
            // Assert.Equal(LocationType.Region,     region.Type);

        }

        [Fact]
        public void MultiRegionalTests()
        {
            Assert.Equal(LocationType.MultiRegion, Locations.Google_Asia.Type);
            Assert.Equal(LocationType.MultiRegion, Locations.Google_EU.Type);
            Assert.Equal(LocationType.MultiRegion, Locations.Google_US.Type);
        }

        [Fact]
        public void TypeTests()
        {
            Assert.Equal(LocationType.Region, Locations.Amazon_AP_NorthEast1.Type);
            Assert.Equal(LocationType.Zone, Locations.Amazon_AP_NorthEast1.WithZone('A').Type);
        }

        [Fact]
        public void LocationEquality()
        {
            var a1 = new LocationInfo(1, "a"); 
            var a2 = new LocationInfo(1, "a");
            var b1 = new LocationInfo(1, "b");

            Assert.True(a1.Equals(a2));
            Assert.False(a1.Equals(b1));
        }

        [Fact]
        public void LocationIdTest()
        {
            var ab = LocationId.Create(16777472); // Amazon_USEast1

            Assert.Equal(1,        ab.ProviderId);   // Amazon
            Assert.Equal(1,        ab.RegionNumber); // 1
            Assert.Equal(0,        ab.ZoneNumber);   // 0
            Assert.Equal(16777472, ab.Value);

            Assert.Equal(16777473, Locations.Amazon_US_East1.WithZone('A').Id);
            Assert.Equal(16777474, Locations.Amazon_US_East1.WithZone('B').Id);
            Assert.Equal(16777475, Locations.Amazon_US_East1.WithZone('C').Id);
            Assert.Equal(16777476, Locations.Amazon_US_East1.WithZone('D').Id);
            Assert.Equal(16777477, Locations.Amazon_US_East1.WithZone('E').Id);

            var zone = new Location(ab, "us-east-1");

            Assert.Equal(ResourceProvider.Amazon.Id, zone.ProviderId);
            Assert.Equal(LocationType.Region, zone.Type);
        }

        [Fact]
        public void WithZoneTests()
        {
            var a = Locations.Amazon_US_East1.WithZone('A');

            Assert.Equal(1, a.ProviderId);
            Assert.Equal(16777473, a.Id);
            Assert.Equal("us-east-1a", a.Name);

            Assert.Equal("asia-east1-c", Locations.Google_AsiaEast1.WithZone('C').Name);
            Assert.Equal("asia-east1-d", Locations.Google_AsiaEast1.WithZone('d').Name);
        }

        [Fact]
        public void Ids()
        {
            Assert.Equal(16777472, Locations.Amazon_US_East1.Id);
            Assert.Equal(16777473, Locations.Amazon_US_East1.WithZone('A').Id);
            Assert.Equal(16777474, Locations.Amazon_US_East1.WithZone('B').Id);
        }
    }
}