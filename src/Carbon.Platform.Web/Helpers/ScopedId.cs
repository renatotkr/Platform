using System;

namespace Carbon.Platform
{
    internal struct ScopedId
    {
        const int ScopeBits    = 42; // ~4.39trillion       
        const int SequenceBits = 22; // 4,194,303 

        const ulong SequenceMask = -1L ^ (-1L << SequenceBits);
        const ulong ScopeMask = ulong.MaxValue ^ SequenceMask; // 2**42 << 8

        const int MaxSequence = (int)SequenceMask; //  4,194,303 

        public ScopedId(long scopeId, int sequenceNumber)
        {
            ScopeId = scopeId;
            SequenceNumber = sequenceNumber;
        }

        public long ScopeId { get; }

        public int SequenceNumber { get; }
    
        public static Range GetRange(long scopeId)
        {
            return new Range(
                start: Create(scopeId, 0),
                end: Create(scopeId, MaxSequence)
            );
        }
        

        public static long Create(long scopeId, int sequence)
        {
            return (long)Create((ulong)scopeId, (ulong)sequence);
        }

        public static ulong Create(ulong scopeId, ulong sequence)
        {
            if (sequence > SequenceMask)
            {
                throw new Exception("Sequence must be less than:" + SequenceMask);
            }

            var value = (scopeId << SequenceBits) | sequence;

            return value;
        }

        public static long GetScope(long value)
        {
            return (long)((ulong)value >> SequenceBits);
        }

        public static ScopedId FromValue(long value)
        {
            return FromValue((ulong)value);
        }

        public static ScopedId FromValue(ulong value)
        {
            var accountId = value >> SequenceBits;
            var sequenceId = value & SequenceMask;

            return new ScopedId((long)accountId, (int)sequenceId);
        }
    }

    public struct Range
    {
        public Range(long start, long end)
        {
            Start = start;
            End = end;
        }

        public long Start { get; }

        public long End { get; }
    }
}
