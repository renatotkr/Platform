﻿using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Sequences
{
    [DataContract]
    internal readonly struct ScopedId
    {
        const int ScopeBits    = 42; // ~4.39trillion       
        const int SequenceBits = 22; // 4,194,303 

        const ulong SequenceMask = -1L ^ (-1L << SequenceBits);
        const ulong ScopeMask    = ulong.MaxValue ^ SequenceMask; // 2**40 << 8

        public const long MaxScope   = 2199023255552;     
        public const int MaxSequence = (int)SequenceMask;

        public ScopedId(ulong value)
        {
            Value = value;
        }

        public ScopedId(long value)
        {
            Value = (ulong)value;
        }

        [DataMember(Name = "value", Order = 1)]
        public readonly ulong Value;

        public long ScopeId => (long)(Value >> SequenceBits);

        public int SequenceId => (int)(Value & SequenceMask);

        public static Range GetRange(long scopeId)
        {
            return new Range(
                start : Create(scopeId, 0),
                end   : Create(scopeId, MaxSequence)
            );
        }

        public static long Create(long scopeId, int sequence)
        {
            return (long)Create((ulong)scopeId, (ulong)sequence);
        }

        public static ulong Create(ulong scopeId, ulong sequenceNumber)
        {
            #region Preconditions

            if (sequenceNumber > SequenceMask)
            {
                throw new Exception("Sequence must be less than:" + SequenceMask);
            }

            #endregion

            var value = (scopeId << SequenceBits) | sequenceNumber;

            return value;
        }

        public static long GetScope(long value)
        {
            return (long)((ulong)value >> SequenceBits);
        }
    }
}