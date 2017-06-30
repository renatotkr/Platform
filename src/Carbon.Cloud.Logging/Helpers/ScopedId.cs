using System;

namespace Carbon.Data.Sequences
{
    internal struct ScopedId
    {
        const int ScopeBits = 42; // ~4.39trillion [139 years in milliseconds]    
        const int SequenceBits = 22; // 4,194,303 

        const ulong SequenceMask = -1L ^ (-1L << SequenceBits);
        const ulong ScopeMask = ulong.MaxValue ^ SequenceMask; // 2**40 << 8

        public const long MaxScope = 2199023255552;     // ~2.19 trillion
        public const int MaxSequenceNumber = (int)SequenceMask; //  4,194,303

        public ScopedId(ulong value)
        {
            Value = value;
        }

        public ulong Value { get; }

        public long ScopeId => (long)(Value >> SequenceBits);

        public int SequenceNumber => (int)(Value & SequenceMask);

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
    }
}