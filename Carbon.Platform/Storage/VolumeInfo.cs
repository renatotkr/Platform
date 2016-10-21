using System;

namespace Carbon.Storage
{
    using Data.Annotations;

    [Dataset("Volumes")]
    public class VolumeInfo
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2), Unique] // guid by default?
        public string Name { get; set; }

        [Member(3)]
        public DriveType Type { get; set; }

        [Member(4)]
        public VolumeStatus Status { get; }

        [Member(5)]
        public long ZoneId { get; }

        [Member(6)]
        public long Size { get; set; } // in octets
        
        [Member(7), Optional, Indexed]
        public long? SourceId { get; set; }

        [Member(8), Mutable]
        public long FreeSpace { get; set; }

        [Member(9), Mutable, Indexed] // Can volumes be shared between hosts?
        public long? HostId { get; set; }

        [Member(10), Mutable]
        public string DeviceName { get; set; } // e.g. D, dev/disk1
         
        [Member(11)]
        public long? CreatorId { get; set; }

        [Member(12), Timestamp] // snapshot date if from source
        public DateTime Created { get; }
    }

    // A volume may also be a snapshot or image...

    // EncrpytionKey
}