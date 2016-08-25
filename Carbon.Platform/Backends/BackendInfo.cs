using System.Collections.Generic;

namespace Carbon.Platform
{
    using Data.Annotations;
    using Hosting;
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

        [Member(5, mutable: true)]
        public Semver ProgramVersion { get; set; }

        [Member(6, mutable: true)]
        public long RequestCount { get; set; }

        public UrlRoute Routes { get; set; }

        public IList<Process> Processes { get; set; }
    }
}

// A backend service....

// - spawns one or more processes to handle user requests (autoscales)
// - hosted behind a load balancer
// - responsible for monitoring the health of the processes (taking them into and out of service as needed)

// bindings (host, protocal, port)