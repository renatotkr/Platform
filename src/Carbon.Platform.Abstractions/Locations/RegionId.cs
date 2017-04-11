using System.Runtime.InteropServices;

namespace Carbon.Platform
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct RegionId
    {
        [FieldOffset(0)] // 255 zones
        public byte ZoneNumber;

        [FieldOffset(1)] // 65K regions
        public ushort RegionNumber;

        [FieldOffset(3)] // 255 providers
        public byte ProviderId;

        [FieldOffset(0)]
        public int Value;

        public static implicit operator int (RegionId id) => id.Value;

        public static RegionId Create(int id)
        {
            return new RegionId { Value = id };
        }

        public static RegionId Create(LocationId id)
        {
            return new RegionId {
                ProviderId   = (byte) id.ProviderId,
                RegionNumber = id.RegionNumber,
                ZoneNumber   = id.ZoneNumber
            };
        }

        public static RegionId Create(
            int providerId,
            int regionNumber,
            int zoneNumber = 0
        ) {
            return new RegionId {
                ProviderId   = (byte)providerId,
                RegionNumber = (ushort)regionNumber,
                ZoneNumber   = (byte)zoneNumber
            };
        }
    }
}

// ProviderId     1     // 255
// RegionNumber   2     // 65K 
// ZoneId         1     // 255


/*
Notes:
336 cities with over 1M people
1,127 cities with at least 500,000 inhabits | 2.317 billion people in these cities
4,116 cities with at least 100,000 people 
*/

