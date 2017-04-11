using System;
using System.Runtime.InteropServices;

namespace Carbon.Platform.Diagnostics.Identity
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct BigId : IEquatable<BigId>
    {
        [FieldOffset(0)]
        public long scopeId;

        // sinces since 1970 -- gives us until 2,106
        [FieldOffset(8)]
        public uint timestamp;

        [FieldOffset(12)]
        public uint sequence;

        public long ScopeId => scopeId;

        public DateTime GetTimestamp()
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
        }

        public byte[] Serialize()
        {
            byte[] data = new byte[16];

            var a = BitConverter.GetBytes(scopeId);
            var b = BitConverter.GetBytes(timestamp);
            var c = BitConverter.GetBytes(sequence);

            Array.Copy(a, 0, data, 0, 8);
            Array.Copy(b, 0, data, 8, 4);
            Array.Copy(c, 0, data, 12, 4);

            return data;
        }

        public static BigId Parse(string text)
        {
            var bytes = Convert.FromBase64String(text);

            return Deserialize(bytes);
        }

        public override string ToString()
        {
            return Convert.ToBase64String(Serialize());
        }

        public static BigId Create(long scopeId, uint timestamp, uint sequence) =>
            new BigId
            {
                scopeId = scopeId,
                timestamp = timestamp,
                sequence = sequence
            };

        public static BigId Deserialize(byte[] data) =>
            new BigId
            {
                scopeId = BitConverter.ToInt64(data, 0),
                timestamp = BitConverter.ToUInt32(data, 8),
                sequence = BitConverter.ToUInt32(data, 12)
            };

        public bool Equals(BigId other) =>
            scopeId == other.scopeId &&
            timestamp == other.timestamp &&
            sequence == other.sequence;
    }

    // Timestamp = 48
    // Random    = 16 
}