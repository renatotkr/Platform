using System;

namespace Carbon.Platform.Backends
{
    using Data.Annotations;
    using Versioning;

    // Webapp
    // Worker

    [Dataset("Backends")]
    public class Backend : IBackend
    {
        [Member("id"), Identity] // 1-1 w/ program?
        public long Id { get; set; }

        [Member("name"), Unique]
        [StringLength(50)]
        public string Name { get; set; } // e.g. carbonmade

        [Member("type")]
        public BackendType Type { get; }

        [Member("programId")]
        public long ProgramId { get; set; }

        [Member("programRelease")]
        public SemanticVersion ProgramVersion { get; set; }

        [Member("env")] // JSON encoded environment data
        public string Env { get; set; }
        
        // public long ImageId { get; set; }

        [Member("modified"), Timestamp]
        public DateTime Modified { get; set; }
    }
}

/*
 
{
   listener: "http://*:80",
   machineType: "t2.xlarge"
   ...
}

 */

// An backend exposes a program as a load balanced worker or web service

// Web Role
// - spawns one or more instances to handle user requests (autoscaling) [using containers, as needed]
// - hosted behind a load balancer

// bindings (host, protocal, port)

// Worker Role
// - Scales to queue...