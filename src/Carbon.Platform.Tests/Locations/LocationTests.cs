﻿using Xunit;

namespace Carbon.Platform.Tests
{
    using static ResourceProvider;

    public class LocationTests
    {
        [Fact]
        public void BorgZones()
        {
            Assert.Equal(16777472, LocationId.Create(Borg, 1));
            Assert.Equal(16777217, LocationId.Create(Borg, 0, 1));
        }

        [Fact]
        public void GlobalTest()
        {
            var global = Locations.Global;

            Assert.Equal(0, global.Id);
        }

        [Fact]
        public void AwsZones()
        {
            foreach (var letter in new[] { 'a', 'b', 'c', 'd', 'e' })
            {
                var name = "us-east-1" + letter;

                var location = Locations.Get(Aws, name);

                Assert.Equal(letter.ToString(), location.ZoneName);

                var l2 = Locations.Get(location.Id);

                Assert.Equal(name,              l2.Name);
                Assert.Equal(letter.ToString(), l2.ZoneName);
                Assert.Equal(LocationType.Zone, l2.Type);
            }
        }
        
        [Fact]
        public void LocationRange()
        {
            var s = LocationId.Create(Aws, 1, 000);
            var e = LocationId.Create(Aws, 1, 255);

            Assert.Equal(33554688, s.Value);
            Assert.Equal(33554943, e.Value);
        }

        [Fact]
        public void MultiRegion()
        {
            var eu = Locations.Gcp.EU;

            var id = LocationId.Create(eu.Id);

            Assert.Equal(3, id.ProviderId);
            Assert.Equal(0, id.RegionNumber);
            Assert.Equal(2, id.ZoneNumber);
        }

        [Fact]
        public void GetTests()
        {
            Assert.Equal(Locations.Get(Locations.Gcp.Asia.Id), Locations.Gcp.Asia);
            Assert.Equal(Locations.Get(Locations.Gcp.EU.Id),   Locations.Gcp.EU);
            Assert.Equal(Locations.Get(Locations.Gcp.US.Id),   Locations.Gcp.US);

            Assert.Equal(Locations.Get(Locations.Aws.USEast1.Id),      Locations.Aws.USEast1);
            Assert.Equal(Locations.Get(Locations.Aws.USWest1.Id),      Locations.Aws.USWest1);
            Assert.Equal(Locations.Get(Locations.Aws.USWest2.Id),      Locations.Aws.USWest2);
            Assert.Equal(Locations.Get(Locations.Aws.APNorthEast1.Id), Locations.Aws.APNorthEast1);
        }

        [Fact]
        public void TestProperties()
        {
            var region = new LocationInfo(Locations.Aws.USEast1.Id, "us-east-1");

            Assert.Equal(Aws,                 region.Provider);
            Assert.Equal("us-east-1",         region.Name);
            Assert.Equal(LocationType.Region, region.Type);
        }

        [Fact]
        public void MultiRegionalTests()
        {
            Assert.Equal(LocationType.MultiRegion, Locations.Gcp.Asia.Type);
            Assert.Equal(LocationType.MultiRegion, Locations.Gcp.EU.Type);
            Assert.Equal(LocationType.MultiRegion, Locations.Gcp.US.Type);
        }

        [Fact]
        public void TypeTests()
        {
            Assert.Equal(LocationType.Region, Locations.Aws.APNorthEast1.Type);
            Assert.Equal(LocationType.Zone,   Locations.Aws.APNorthEast1.WithZone('A').Type);
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

            Assert.Equal(33554689, Locations.Aws.USEast1.WithZone('A').Id);
            Assert.Equal(33554690, Locations.Aws.USEast1.WithZone('B').Id);
            Assert.Equal(33554691, Locations.Aws.USEast1.WithZone('C').Id);
            Assert.Equal(33554692, Locations.Aws.USEast1.WithZone('D').Id);
            Assert.Equal(33554693, Locations.Aws.USEast1.WithZone('E').Id);

            var zone = new Location(ab, "us-east-1");

            Assert.Equal(Aws.Id,              zone.ProviderId);
            Assert.Equal(LocationType.Region, zone.Type);
        }

        [Fact]
        public void WithZoneTests()
        {
            var a = Locations.Aws.USEast1.WithZone('A');

            Assert.Equal(2, a.ProviderId);
            Assert.Equal(33554689, a.Id);
            Assert.Equal("us-east-1a", a.Name);

            Assert.Equal("asia-east1-c", Locations.Gcp.Asia_East1.WithZone('C').Name);
            Assert.Equal("asia-east1-d", Locations.Gcp.Asia_East1.WithZone('d').Name);
        }

        [Fact]
        public void Ids()
        {
            Assert.Equal(33554688, Locations.Aws.USEast1.Id);
            Assert.Equal(33554689, Locations.Aws.USEast1.WithZone('A').Id);
            Assert.Equal(33554690, Locations.Aws.USEast1.WithZone('B').Id);
        }
    }
}