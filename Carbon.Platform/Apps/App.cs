using System.Collections.Generic;

namespace Carbon.Computing
{
    using Data.Annotations;
    using Repositories;

    [Dataset("Apps")]
    public class App : IProgram
    {
        [Member(1), Key]  // Assigned (1-1 w/ Program)
        public long Id { get; set; }

        [Member(2), Mutable] // Current version
        public Semver Version { get; set; }

        [Member(3), Unique]
        public string Name { get; set; } // e.g. carbonmade

        [Member(4)]
        public RepositoryInfo Repository { get; set; }

        public IList<AppInstance> Instances { get; set; }
    }
}

// An app exposes a program as service

// - spawns one or more instances to handle user requests (autoscales)
// - hosted behind a load balancer

// bindings (host, protocal, port)