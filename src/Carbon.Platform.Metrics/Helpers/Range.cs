using System.Runtime.Serialization;

namespace Carbon.Platform.Sequences
{
    [DataContract]
    internal struct Range
    {
        public Range(long start, long end)
        {
            Start = start;
            End = end;
        }

        [DataMember(Name = "start", Order = 1)]
        public long Start { get; }

        [DataMember(Name = "end", Order = 2)]
        public long End { get; }
    }
}