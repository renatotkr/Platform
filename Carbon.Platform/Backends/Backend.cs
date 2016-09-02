using System.Collections.Generic;

namespace Carbon.Computing
{
    using Data.Annotations;
    using Hosting;
    using Networking;

    [Dataset("Backends")]
    public class Backend
    {
        [Member(1), Identity]
        public long Id { get; set; }
        
        [Member(2), Unique]
        public string Name { get; set; } // e.g. carbonmade

        [Member(3)]
        public long ProgramId { get; set; }

        [Member(5), Mutable]
        public Semver ProgramVersion { get; set; }

        [Member(6), Mutable]
        public long RequestCount { get; set; }

        public IList<UrlRoute> Routes { get; set; }

        public IList<Process> Processes { get; set; }
    }
}

// A backend exposes a program over a service (http or borg) .... 

// - spawns one or more processes to handle user requests (autoscales)
// - hosted behind a load balancer
// - responsible for monitoring the health of the processes (taking them into and out of service as needed)

// bindings (host, protocal, port)