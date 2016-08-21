namespace Carbon.Platform
{
    using Data.Annotations;
    using System;

    [Record(TableName = "Volumes")]
    public class VolumeInfo
    {
        [Identity]
        public long Id { get; set; }

        [Mutable, Indexed] // Can volumes be shared between hosts?
        public long HostId { get; set; }

        [Mutable]
        public string MountName { get; set; } // MountPath

        [Mutable]
        public long FreeSpace { get; set; }

        // in octets
        public long Size { get; set; }

        public long ZoneId { get; }

        [Optional, Indexed]
        public long? SourceId { get; set; }
        
        [Version(false)] // snapshot date if from source
        public DateTime Created { get; }
    }


    // EncrpytionKey
 
    // snapshot / source?
}