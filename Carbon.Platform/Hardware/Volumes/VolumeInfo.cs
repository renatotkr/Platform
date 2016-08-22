using System;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Volumes")]
    public class VolumeInfo
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2)]
        public long Size { get; set; } // in octets

        [Member(3)]
        public long ZoneId { get; }

        [Member(4), Optional, Indexed]
        public long? SourceId { get; set; }

        [Member(5, Mutable = true)]
        public long FreeSpace { get; set; }

        [Member(6, Mutable = true), Indexed] // Can volumes be shared between hosts?
        public long HostId { get; set; }

        [Member(7, Mutable = true)]
        public string MountName { get; set; } // MountPath
         
        [Member(8), Version(false)] // snapshot date if from source
        public DateTime Created { get; }
    }


    // EncrpytionKey
 
    // snapshot / source?
}