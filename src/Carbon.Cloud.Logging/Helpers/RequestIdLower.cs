using System;

namespace Carbon.Cloud.Logging
{
    internal struct RequestIdLower
    {
        const int MillisecondBits = 22;   
        const int SequenceBits    = 42;

        const ulong SequenceMask = ulong.MaxValue ^ (ulong.MaxValue << SequenceBits);
        const ulong NanoMask     = ulong.MaxValue ^ SequenceMask;
        
        public RequestIdLower(ulong value)
        {
            Value = value;
        }

        public ulong Value { get; }

        public long Milliseconds => (long)(Value >> SequenceBits);

        public long SequenceNumber => (long)(Value & SequenceMask);
        
        public static ulong Create(ulong milliseconds, ulong sequence)
        {
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