using System.Runtime.Serialization;

namespace Carbon.Platform.Sequences
{
    [DataContract]
    internal readonly struct Range
    {
        public Range(long start, long end)
        {
            Start = start;
            End = end;
        }

        [DataMember(Name = "start", Order = 1)]
        public readonly long Start;

        [DataMember(Name = "end", Order = 2)]
        public readonly long End;
    }
}