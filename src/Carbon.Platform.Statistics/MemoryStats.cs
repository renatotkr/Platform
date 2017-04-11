using System.Runtime.Serialization;

namespace Carbon.Platform
{
    [DataContract]
    public class MemoryStats
    {
        public MemoryStats(long used, long total)
        {
            UsedBytes = used;
            TotalBytes = total;
        }

        public MemoryStats() { }

        [DataMember(Order = 1)]
        public long UsedBytes { get; set; }

        [DataMember(Order = 2)]
        public long TotalBytes { get; set; }

        // AvailableBytes?
    }
}

// Note: send & receive rates are aggergate