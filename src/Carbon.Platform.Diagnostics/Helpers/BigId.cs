using System;
using System.Runtime.InteropServices;

namespace Carbon.Data.Sequences
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct BigId : IEquatable<BigId>
    {
        [FieldOffset(0)]
        private long upperHalf;

        [FieldOffset(8)]
        private ulong lowerHalf;

        public long ScopeId => upperHalf;

        // milliseconds since 1970
        public long Timestamp => new ScopedId(lowerHalf).ScopeId;

        // max: 4,194,303 
        public int SequenceNumber => new ScopedId(lowerHalf).SequenceNumber;

        public DateTime GetTimestamp() => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).UtcDateTime;

        public static BigId Create(long scopeId, DateTime timestamp, int sequenceNumber)
        {
            return Create(
                scopeId: scopeId,
                timestamp: new DateTimeOffset(DateTime.SpecifyKind(timestamp, DateTimeKind.Utc)),
                sequence: sequenceNumber
            );
        }

        public static BigId Create(long scopeId, DateTimeOffset timestamp, int sequence)
        {
            #region Preconditions

            if (sequence > ScopedId.MaxSequenceNumber)
            {
                throw new ArgumentOutOfRangeException(nameof(sequence), sequence, $"Must be between 0 & {ScopedId.MaxSequenceNumber}");
            }

            #endregion

            // use a ulong before 2039
            var lowerHalf = ScopedId.Create(timestamp.ToUnixTimeMilliseconds(), sequence);

            return new BigId
            {
                upperHalf = scopeId,
                lowerHalf = (ulong)lowerHalf
            };
        }

       
        #region Serialization

        // intel: little-endian

        // db = big-endian
        public static BigId Deserialize(byte[] data)
        {
            if (data.Length != 16)
                throw new Exception("Must be 16 bytes");

            var a = new byte[8];
            var b = new byte[8];

            Array.Copy(data, 0, a, 0, 8);
            Array.Copy(data, 8, b, 0, 8);

            Array.Reverse(a);
            Array.Reverse(b);

            return new BigId
            {
                upperHalf = BitConverter.ToInt64(a, 0),
                lowerHalf = BitConverter.ToUInt64(b, 0)
            };
        }

        public byte[] Serialize()
        {
            byte[] data = new byte[16];

            var a = BitConverter.GetBytes(upperHalf);
            var b = BitConverter.GetBytes(lowerHalf);

            Array.Reverse(a);
            Array.Reverse(b);

            Array.Copy(a, 0, data, 0, 8);
            Array.Copy(b, 0, data, 8, 8);

            return data;
        }

        #endregion

        #region Equality

        public static bool operator ==(BigId lhs, BigId rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(BigId lhs, BigId rhs)
        {
            return !lhs.Equals(rhs);
        }

        public bool Equals(BigId other) =>
            upperHalf == other.upperHalf &&
            lowerHalf == other.lowerHalf;

        public override bool Equals(object obj)
        {
            return obj is BigId id && id.Equals(this);
        }

        public override int GetHashCode()
        {
            return (upperHalf, lowerHalf).GetHashCode();
        }

        #endregion
    }
}