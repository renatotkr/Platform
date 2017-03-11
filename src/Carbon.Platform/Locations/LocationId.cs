using System.Runtime.InteropServices;

namespace Carbon.Platform
{
    // ~4B clouds w/ ~65K Regions w/ 255 zones
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct LocationId
    {
        [FieldOffset(0)]
        public byte ZoneNumber;

        [FieldOffset(1)]
        public byte Flags;

        [FieldOffset(2)]
        public ushort RegionNumber;

        [FieldOffset(4)]
        public int ProviderId;

        [FieldOffset(0)]
        public long Value;

        public static implicit operator long (LocationId id) => id.Value;

        public static LocationId Create(CloudProvider provider, ushort regionNumber, byte zoneNumber = 0, byte flags = 0) =>
            new LocationId {
                ProviderId   = provider.Id,
                RegionNumber = regionNumber,
                ZoneNumber   = zoneNumber,
                Flags        = flags
            };

        public static LocationId Create(long value) =>
            new LocationId { Value = value };
    }

    public enum LocationFlags : byte
    {
        None          = 0,
        Global        = 1 << 1,
        MultiRegional = 1 << 2
    }
}


/*
Notes:
336 cities with over 1M people
1,127 cities with at least 500,000 inhabits | 2.317 billion people in these cities
4,116 cities with at least 100,000 people 
*/

