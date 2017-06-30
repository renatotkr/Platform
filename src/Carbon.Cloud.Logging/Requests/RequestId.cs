using System;

using Carbon.Data.Sequences;

namespace Carbon.Cloud.Logging
{
    public static class RequestId
    {
        public static Uid Create(long scopeId, DateTime timestamp, int sequenceNumber)
        {
            return Create(
                scopeId   : scopeId,
                timestamp : new DateTimeOffset(DateTime.SpecifyKind(timestamp, DateTimeKind.Utc)),
                sequence  : sequenceNumber
            );
        }

        public static Uid Create(long scopeId, DateTimeOffset timestamp, int sequence)
        {
            #region Preconditions

            if (sequence > ScopedId.MaxSequenceNumber)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sequence),
                    sequence, $"Must be between 0 and {ScopedId.MaxSequenceNumber}");
            }

            #endregion

            // change to a ulong before 2039
            var lowerHalf = ScopedId.Create(timestamp.ToUnixTimeMilliseconds(), sequence);

            return new Uid(
                upper: (ulong)scopeId,
                lower: (ulong)lowerHalf
            );
        }
    }
}
