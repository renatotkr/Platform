using System.Runtime.InteropServices;

namespace Carbon.Platform.Computing
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct HostId
    {
        [FieldOffset(0)]    
        private int sequenceNumber;

        [FieldOffset(4)]
        private int locationId;

        [FieldOffset(0)]
        private long value;

        public int SequenceNumber => sequenceNumber;

        public int LocationId => locationId;

        public long Value => value;
        
        public static implicit operator long(HostId id) => id.Value;

        public static HostId Get(long value)
        {
            return new HostId { value = value };
        }

        public static HostId Create(ILocation location, int sequenceNumber)
        {
            return new HostId {
                sequenceNumber = sequenceNumber,
                locationId = location.Id
            };
        }

        public static HostId Create(LocationId locationId, int sequenceNumber)
        {
            return new HostId {
                sequenceNumber = sequenceNumber,
                locationId = locationId.Value
            };
        }
    }
}
