using System;

namespace Carbon.Storage
{
    using Data.Annotations;

    [Record(TableName = "Volumes")]
    public class VolumeInfo
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2), Unique] // guid by default?
        public string Name { get; set; }

        [Member(3)]
        public VolumeType Type { get; set; }

        [Member(4)]
        public VolumeStatus Status { get; }

        [Member(5)]
        public long ZoneId { get; }

        [Member(6)]
        public long Size { get; set; } // in octets
        
        [Member(7), Optional, Indexed]
        public long? SourceId { get; set; }

        [Member(8, mutable: true)]
        public long FreeSpace { get; set; }

        [Member(9, mutable: true), Indexed] // Can volumes be shared between hosts?
        public long? HostId { get; set; }

        [Member(10, mutable: true)]
        public string DeviceName { get; set; } // e.g. D, dev/disk1
         
        [Member(11)]
        public long? CreatorId { get; set; }

        [Member(12), Timestamp(false)] // snapshot date if from source
        public DateTime Created { get; }
    }

    public enum VolumeType
    {
        SSD    = 1,
        Optane = 2
    }

    // A volume may also be a snapshot or image...

    // EncrpytionKey
}