using System;
using System.Runtime.InteropServices;

namespace Carbon.Platform
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct LocationId
    {
        public static readonly LocationId Zero = new LocationId { value = 0 };

        [FieldOffset(0)] // 255 zones
        private byte zoneNumber;

        [FieldOffset(1)] // 65K regions
        private ushort regionNumber;

        [FieldOffset(3)] // 255 providers
        private byte providerId;

        [FieldOffset(0)]
        private int value;

        public byte ProviderId => providerId;

        public ushort RegionNumber => regionNumber;

        public byte ZoneNumber => zoneNumber;

        public int Value => value;

        #region Helpers

        public LocationId WithZoneNumber(byte zoneNumber)
        {
            var id = Create(Value);

            id.zoneNumber = zoneNumber;

            return id;
        }

        #endregion

        #region Type

        public LocationType Type
        {
            get
            {
                if (regionNumber == 0)
                {
                    return zoneNumber == 0
                        ? LocationType.Global
                        : LocationType.MultiRegion;
                }

                return zoneNumber == 0
                    ? LocationType.Region
                    : LocationType.Zone;
            }
        }

        #endregion

        public static implicit operator int (LocationId id) => id.Value;

        public static LocationId Create(int id)
        {
            return new LocationId { value = id };
        }      

        public static LocationId Create(
            ResourceProvider provider,
            ushort regionNumber,
            byte zoneNumber = 0)
        {
            return Create(provider.Id, regionNumber, zoneNumber);
        }

        public static LocationId Create(
            int providerId,
            ushort regionNumber,
            byte zoneNumber = 0
        )
        {
            #region Preconditions

            if (providerId < 0 || providerId >= 128)
            {
                throw new ArgumentOutOfRangeException("providerId", providerId, "Must be between 0 and 127");
            }

            #endregion

            return new LocationId {
                providerId   = (byte)providerId,
                regionNumber = regionNumber,
                zoneNumber   = zoneNumber
            };
        }
    }
}


/*
Notes:
336 cities with over 1M people
1,127 cities with at least 500,000 inhabits | 2.317 billion people in these cities
4,116 cities with at least 100,000 people 
*/

// ProviderId     1     // 255
// RegionNumber   2     // 65K 
// ZoneId         1     // 255


// Region = 0 (Global)
// ZoneA  = US