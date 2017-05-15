using Xunit;

namespace Carbon.Platform.Tests
{
    public class CloudLocationTests
    {
        [Fact]
        public void BorgZones()
        {
            Assert.Equal(16777472, LocationId.Create(ResourceProvider.Borg, 1));
            Assert.Equal(16777217, LocationId.Create(ResourceProvider.Borg, 0, 1));
        }

        [Fact]
        public void AllZones()
        {
            foreach (var letter in new[] { 'a', 'b', 'c', 'd', 'e' })
            {
                var location = Locations.Get(ResourceProvider.Aws, "us-east-1" + letter);

                Assert.Equal(letter.ToString(), location.ZoneName);
            }
        }

        [Fact]
        public void AwsRegions1()
        {
            var location = Locations.Get(ResourceProvider.Aws, "us-east-1");
            
            Assert.Equal("us-east-1", location.Name);
            Assert.Equal("us-east-1", location.RegionName);
            Assert.Equal(ResourceProvider.Aws.Id, location.ProviderId);
            Assert.Equal(LocationType.Region, location.Type);
        }

        [Fact]
        public void AwsRegion1WithZone()
        {
            var location = Locations.Get(ResourceProvider.Aws, "us-east-1a");
            
            Assert.Equal("us-east-1", location.RegionName);
            Assert.Equal("us-east-1a", location.Name);
            Assert.Equal("a", location.ZoneName);

            // Assert.Equal(ResourceProvider.Amazon.Id, location.ProviderId);
            Assert.Equal(LocationType.Zone, location.Type);
        }

        [Fact]
        public void LocationRange()
        {
            var s = LocationId.Create(ResourceProvider.Aws, 1, 000);
            var e = LocationId.Create(ResourceProvider.Aws, 1, 255);


            Assert.Equal(33554688, s.Value);
            Assert.Equal(33554943, e.Value);
        }

        [Fact]
        public void MultiRegion()
        {
            var eu = Locations.Gcp_EU;

            var id = LocationId.Create(eu.Id);

            Assert.Equal(3, id.ProviderId);
            Assert.Equal(0, id.RegionNumber);
            Assert.Equal(2, id.ZoneNumber);
        }

        [Fact]
        public void GetTests()
        {
            Assert.Equal(Locations.Get(Locations.Gcp_Asia.Id), Locations.Gcp_Asia);
            Assert.Equal(Locations.Get(Locations.Gcp_EU.Id),   Locations.Gcp_EU);
            Assert.Equal(Locations.Get(Locations.Gcp_US.Id),   Locations.Gcp_US);

            Assert.Equal(Locations.Get(Locations.Aws_US_East_1.Id), Locations.Aws_US_East_1);
            Assert.Equal(Locations.Get(Locations.Aws_US_West_1.Id), Locations.Aws_US_West_1);
            Assert.Equal(Locations.Get(Locations.Aws_US_West_2.Id), Locations.Aws_US_West_2);
            Assert.Equal(Locations.Get(Locations.Aws_AP_NorthEast_1.Id), Locations.Aws_AP_NorthEast_1);
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
            var region = new LocationInfo(Locations.Aws_US_East_1.Id, "us-east-1");

            Assert.Equal(ResourceProvider.Aws, region.Provider);
            Assert.Equal("us-east-1",             region.Name);
            Assert.Equal(LocationType.Region,     region.Type);
        }

        [Fact]
        public void MultiRegionalTests()
        {
            Assert.Equal(LocationType.MultiRegion, Locations.Gcp_Asia.Type);
            Assert.Equal(LocationType.MultiRegion, Locations.Gcp_EU.Type);
            Assert.Equal(LocationType.MultiRegion, Locations.Gcp_US.Type);
        }

        [Fact]
        public void TypeTests()
        {
            Assert.Equal(LocationType.Region, Locations.Aws_AP_NorthEast_1.Type);
            Assert.Equal(LocationType.Zone,   Locations.Aws_AP_NorthEast_1.WithZone('A').Type);
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
            var ab = LocationId.Create(33554688); // Amazon_USEast1

            Assert.Equal(2,        ab.ProviderId);   // Amazon
            Assert.Equal(1,        ab.RegionNumber); // 1
            Assert.Equal(0,        ab.ZoneNumber);   // 0
            Assert.Equal(33554688, ab.Value);

            Assert.Equal(33554689, Locations.Aws_US_East_1.WithZone('A').Id);
            Assert.Equal(33554690, Locations.Aws_US_East_1.WithZone('B').Id);
            Assert.Equal(33554691, Locations.Aws_US_East_1.WithZone('C').Id);
            Assert.Equal(33554692, Locations.Aws_US_East_1.WithZone('D').Id);
            Assert.Equal(33554693, Locations.Aws_US_East_1.WithZone('E').Id);

            var zone = new Location(ab, "us-east-1");

            Assert.Equal(ResourceProvider.Aws.Id, zone.ProviderId);
            Assert.Equal(LocationType.Region,        zone.Type);
        }

        [Fact]
        public void WithZoneTests()
        {
            var a = Locations.Aws_US_East_1.WithZone('A');

            Assert.Equal(2, a.ProviderId);
            Assert.Equal(33554689, a.Id);
            Assert.Equal("us-east-1a", a.Name);

            Assert.Equal("asia-east1-c", Locations.Gcp_AsiaEast1.WithZone('C').Name);
            Assert.Equal("asia-east1-d", Locations.Gcp_AsiaEast1.WithZone('d').Name);
        }


        [Fact]
        public void Ids()
        {
            Assert.Equal(33554688, Locations.Aws_US_East_1.Id);
            Assert.Equal(33554689, Locations.Aws_US_East_1.WithZone('A').Id);
            Assert.Equal(33554690, Locations.Aws_US_East_1.WithZone('B').Id);
        }
    }
}