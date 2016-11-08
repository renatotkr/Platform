using System.Collections.Generic;

namespace Carbon.Computing
{
    using Data.Annotations;

    [Dataset("Backends")]
    public class Backend
    {
        [Member(1), Key]  // Assigned (1-1 w/ Program)
        public long Id { get; set; }

        [Member(2), Unique]
        [StringLength(50)]
        public string Name { get; set; } // e.g. carbonmade

        [Member(3)]
        public NetworkPortList Listeners { get; set; } // e.g. 80/http

        [Member(4)]
        public long ProgramId { get; set; }         //  5

        [Member(5), Mutable]
        public Semver Version { get; set; }    // Active version
        
        // Hosts?

        public IList<BackendInstance> Instances { get; set; }
    }
}

// An backend exposes a program as a load balanced service

// - spawns one or more instances to handle user requests (autoscaling)
// - hosted behind a load balancer

// bindings (host, protocal, port)