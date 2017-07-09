﻿using System;

using Carbon.Data.Sequences;

namespace Carbon.Cloud.Logging
{
    // 42        | 22    | 22           | 42
    // accountId | hours | milliseconds | sequence

    public static class RequestId
    {
        

        public static DateTime GetTimestamp(Uid id) // ts/ms
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(
                Timestamp.Combine(GetHours(id), GetMilliseconds(id))
            ).UtcDateTime;
        }

        public static Uid Create(long accountId, DateTime timestamp, long sequenceNumber)
        {
            return Create(
                scopeId        : accountId,
                timestamp      : new DateTimeOffset(DateTime.SpecifyKind(timestamp, DateTimeKind.Utc)),
                sequenceNumber : sequenceNumber
            );
        }

        public static Uid Create(long scopeId, DateTimeOffset timestamp, long sequenceNumber)
        {
            var parts = Timestamp.Split(timestamp.ToUnixTimeMilliseconds());

            return new Uid(
                upper: (ulong)ScopedId.Create(scopeId, (int)parts.hours),
                lower: RequestIdLower.Create((ulong)parts.milliseconds, (ulong)sequenceNumber)
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
            var hours = timestamp / msInHour;
            var milliseconds = (timestamp % msInHour);

            return (hours, milliseconds);
        }

        public static long Combine(long hours, long milliseconds) // ts/ms
        {
            return (hours * msInHour) + milliseconds;

           

        }
    }

    internal struct RequestIdLower
    {
        const int MillisecondBits = 22;   
        const int SequenceBits    = 42;

        const ulong SequenceMask = ulong.MaxValue ^ (ulong.MaxValue << SequenceBits);
        const ulong NanoMask = ulong.MaxValue ^ SequenceMask;

        public RequestIdLower(ulong value)
        {
            Value = value;
        }

        public ulong Value { get; }

        public long Milliseconds => (long)(Value >> SequenceBits);

        public long SequenceNumber => (long)(Value & SequenceMask);


        public static ulong Create(ulong milliseconds, ulong sequence)
        {
            if (milliseconds > 4_194_304)
                throw new Exception("Must be less than 4194304");

            if (sequence > SequenceMask)
                throw new ArgumentOutOfRangeException(nameof(sequence), sequence, "Sequence must be less than:" + SequenceMask);


            return (milliseconds << SequenceBits) | sequence;
        }

        public static long GetMilliseconds(long value)
        {
            return (long)((ulong)value >> SequenceBits);
        }
    }
}
