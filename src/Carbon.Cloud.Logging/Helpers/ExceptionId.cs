using System;

namespace Carbon.Data.Sequences
{
    public static class ExceptionId
    {
        public static DateTime GetTimestamp(this Uid uid)
        {
            var ts = new ScopedId(uid.Lower).ScopeId;

            return DateTimeOffset.FromUnixTimeMilliseconds(ts).UtcDateTime;
        }

        public static Uid Create(long scopeId, DateTime timestamp, int sequenceNumber)
        {
            return Create(
                scopeId: scopeId,
                timestamp: new DateTimeOffset(DateTime.SpecifyKind(timestamp, DateTimeKind.Utc)),
                sequenceNumber: sequenceNumber
            );
        }

        public static Uid Create(long scopeId, DateTimeOffset timestamp, int sequenceNumber)
        {
            #region Preconditions

            if (sequenceNumber > ScopedId.MaxSequenceNumber)
                throw new ArgumentOutOfRangeException(nameof(sequenceNumber), sequenceNumber, $"Must be between 0 & {ScopedId.MaxSequenceNumber}");

            #endregion

            var lower = ScopedId.Create(timestamp.ToUnixTimeMilliseconds(), sequenceNumber);

            return new Uid((ulong)scopeId, (ulong)lower);
        }
    }
}