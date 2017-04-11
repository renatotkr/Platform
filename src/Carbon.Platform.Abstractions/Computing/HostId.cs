using System.Runtime.InteropServices;

namespace Carbon.Platform.Computing
{
    // ProviderId     1     // 255
    // RegionNumber   2     // 65K 
    // ZoneId         1     // 255
    // Sequence       4     // 4B

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct HostId
    {
        [FieldOffset(0)]    
        public int Sequence;

        [FieldOffset(4)]    
        public byte ZoneNumber;

        [FieldOffset(5)]    
        public ushort RegionNumber;

        [FieldOffset(7)]   
        public byte ProviderId;

        [FieldOffset(0)]
        public long Value;

        public static long Create(ILocation location, int sequence)
        {
            return Create(LocationId.Create(location.Id), sequence);
        }

        public static long Create(LocationId locationId, int sequence)
        {
            return new HostId {
                Sequence     = sequence,
                ZoneNumber   = locationId.ZoneNumber,
                RegionNumber = locationId.RegionNumber,
                ProviderId   = (byte)locationId.ProviderId
            }.Value;
        }
    }
}
