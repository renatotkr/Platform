using System;

using Carbon.Data.Sequences;

namespace Carbon.Cloud.Logging
{
    // 42        | 22    | 22           | 42
    // accountId | hours | milliseconds | sequence

    public static class RequestId
    {
        public static Uid Create(long accountId, DateTime timestamp, long sequenceNumber)
        {
            return Create(
                accountId      : accountId,
                timestamp      : new DateTimeOffset(DateTime.SpecifyKind(timestamp, DateTimeKind.Utc)),
                sequenceNumber : sequenceNumber
            );
        }

        public static Uid Create(long accountId, DateTimeOffset timestamp, long sequenceNumber)
        {
            #region Preconditions

            if (accountId < 0)
                throw new ArgumentException("Must be positive", nameof(accountId));

            if (sequenceNumber < 0)
                throw new ArgumentException("Must be positive", nameof(sequenceNumber));

            #endregion

            var (hours, milliseconds) = Timestamp.Split(timestamp.ToUnixTimeMilliseconds());

            return new Uid(
                upper: (ulong)ScopedId.Create(accountId, (int)hours),
                lower: RequestIdLower.Create((ulong)milliseconds, (ulong)sequenceNumber)
            );
        }

        public static long GetAccountId(Uid id)
        {
            return new ScopedId(id.Upper).ScopeId;
        }

        public static long GetSequenceNumber(Uid id)
        {
            return new RequestIdLower(id.Lower).SequenceNumber;
        }

        public static DateTime GetTimestamp(Uid id) // timestamp in ms
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(
                Timestamp.Combine(GetHours(id), GetMilliseconds(id))
            ).UtcDateTime;
        }

        private static long GetHours(Uid id)
        {
            return new ScopedId(id.Upper).SequenceNumber;
        }

        private static long GetMilliseconds(Uid id)
        {
            return new RequestIdLower(id.Lower).Milliseconds;
        }
    }

    internal static class Timestamp
    {
        // 3_600_000 ms in hour (fits in 22 bits [4_194_304])

        const long msInHour = 1000 * 60 * 60;

        // Epoch = 2000
        public static (long hours, long milliseconds) Split(long timestamp) // ts/ms
        {
            var hours        = timestamp / msInHour;
            var milliseconds = (timestamp % msInHour);

            return (hours, milliseconds);
        }

        public static long Combine(long hours, long milliseconds) // ts/ms
        {
            return (hours * msInHour) + milliseconds;
        }
    }
}