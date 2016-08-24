using System.Collections.Generic;

namespace Carbon.Platform
{
    using Data.Annotations;
    using Networking;

    [Record(TableName = "Backends")]
    public class BackendInfo
    {
        [Member(1), Identity]
        public long Id { get; set; }
        
        [Member(2), Unique]
        public string Name { get; set; } // e.g. carbonmade

        [Member(4)]
        public long ProgramId { get; set; }

        [Member(5)]
        public Semver ProgramVersion { get; set; }

        [Member(6, mutable: true)]
        public long RequestCount { get; set; }

        public UrlRoute Routes { get; set; }

        public IList<ProcessInfo> Processes { get; set; }
    }
}

// A backend service....

// - spawns one or more processes to handle user requests (autoscales)
// - hosted behind a load balancer

// bindings (host, protocal, port)