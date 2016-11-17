using System;

namespace Carbon.Computing
{
    using Data.Annotations;

    [Dataset("BackendInstances")]
    public class BackendInstance
    {        
        [Member(1), Key]
        public long BackendId { get; set; }

        [Member(2), Key]
        public long HostId { get; set; }

        [Member(3), Mutable] // Program version
        public SemanticVersion Version { get; set; }

        [Member(4), Mutable]
        public long? ProcessId { get; set; }

        [Member(5)]
        public DateTime? Terminated { get; set; }

        [Member(6), Timestamp]
        public DateTime Modified { get; set; }
    }
}