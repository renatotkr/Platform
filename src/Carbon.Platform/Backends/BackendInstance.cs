using System;

namespace Carbon.Platform.Backends
{
    using Data.Annotations;
    using Versioning;

    [Dataset("BackendInstances")]
    public class BackendInstance
    {        
        [Member("backendId"), Key]
        public long BackendId { get; set; }

        [Member("hostId"), Key]
        public long HostId { get; set; }

        [Member("version"), Mutable] // Program version
        public SemanticVersion Version { get; set; }

        [Member("terminated")]
        public DateTime? Terminated { get; set; }

        [Member("heartbeat")]
        public DateTime? Heartbeat { get; set; }
    }
}