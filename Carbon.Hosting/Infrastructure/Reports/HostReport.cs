namespace Carbon.Platform
{
    using Data.Annotations;

    [Dataset("HostReports")]
    public class HostReport
    {
        [Member(1), Key]
        public int HostId { get; set; }

        [Member(2), Key]
        public long Id { get; set; }

        [Member(3)]
        public float CpuTime { get; set; }

        [Member(4)]
        public long MemoryUsed { get; set; }

        [Member(5)]
        public int ReceiveRate { get; set; }

        [Member(6)]
        public int SendRate { get; set; }
    }
}

// Note: send & receive rates are aggergate