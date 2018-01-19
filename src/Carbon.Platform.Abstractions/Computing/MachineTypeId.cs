using System.Runtime.InteropServices;

namespace Carbon.Platform.Computing
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct MachineTypeId
    {
        // e.g. C, T, M, X, I               | Basic | Standard |  
        [FieldOffset(0)]
        internal byte ClassId1;

        // e.g. I, S
        [FieldOffset(1)]
        internal byte ClassId2;

        // 1, 2, 3, 4, 5, ...
        [FieldOffset(2)]
        internal byte Generation;

        [FieldOffset(3)]
        internal byte MachineSize;

        [FieldOffset(4)]
        internal int ProviderId;

        [FieldOffset(0)]
        public long Value;

        public static implicit operator long(MachineTypeId id) => id.Value;
    }
}