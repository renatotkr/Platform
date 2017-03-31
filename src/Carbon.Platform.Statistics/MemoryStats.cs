using System.Runtime.Serialization;

namespace Carbon.Platform
{
    [DataContract]
    public class MemoryStats
    {
        public MemoryStats(long used, long total)
        {
            BytesUsed = used;
            BytesTotal = total;
        }

        public MemoryStats() { }

        [DataMember(Order = 1)]
        public long BytesUsed { get; set; }

        [DataMember(Order = 2)]
        public long BytesTotal { get; set; }

        // BytesAvailable?
    }
}

// Note: send & receive rates are aggergate