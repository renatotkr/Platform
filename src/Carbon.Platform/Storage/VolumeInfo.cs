using System;

namespace Carbon.Platform.Storage
{
    using Data.Annotations;

    [Dataset("Volumes")]
    public class VolumeInfo
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("description")]
        [StringLength(100)]
        public string Description { get; set; }

        [Member("status"), Mutable]
        public VolumeStatus Status { get; set; }

        [Member("size")]
        public long Size { get; set; } // in octets

        [Member("hostId"), Mutable]
        public long? HostId { get; set; }

        [Member("provider")]
        public PlatformProviderId Provider { get; set; }

        [Member("refId"), Unique]
        [Ascii, StringLength(50)]
        public string RefId { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }
    }
}