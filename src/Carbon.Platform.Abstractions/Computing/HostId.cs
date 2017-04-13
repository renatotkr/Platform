using System.Runtime.InteropServices;

namespace Carbon.Platform.Computing
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct HostId
    {
        [FieldOffset(0)]    
        private int sequence;

        [FieldOffset(4)]
        private int locationId;

        [FieldOffset(0)]
        private long value;

        public int SequenceNumber => sequence;

        public int LocationId => locationId;

        public long Value => value;
        
        public static HostId Get(long value)
        {
            return new HostId { value = value };
        }

        public static long Create(ILocation location, int sequence)
        {
            return new HostId {
                sequence = sequence,
                locationId = location.Id
            }.Value;
        }

        public static long Create(LocationId locationId, int sequence)
        {
            return new HostId {
                sequence   = sequence,
                locationId = locationId.Value
            }.Value;
        }
    }
}
