using System;

namespace Carbon.Computing
{
    using Data.Annotations;
    using Storage;

    [Dataset("Volumes")]
    public class VolumeInfo
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2)]
        public DriveType Type { get; set; }

        [Member(3)]
        public VolumeStatus Status { get; }

        [Member(4)]
        public long ZoneId { get; }

        [Member(5)]
        public long Size { get; set; } // in octets
        
        [Member(6), Indexed]
        public long? SourceId { get; set; }

        [Member(8), Mutable] // Can volumes be shared between hosts?
        [Indexed]
        public long? HostId { get; set; }

        [Member(9), Mutable]
        public string DeviceName { get; set; } // e.g. D, dev/disk1
         
        [Member(10)]
        public long? CreatorId { get; set; }

        [Member(11), Timestamp] // snapshot date if from source
        public DateTime Created { get; }
    }
}