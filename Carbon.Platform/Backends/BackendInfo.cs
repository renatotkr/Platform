using System.Collections.Generic;

namespace Carbon.Platform
{
    using Data.Annotations;
    
    [Record(TableName = "Backends")]
    public class BackendInfo
    {
        [Member(1), Identity]
        public long Id { get; set; }
        
        [Member(2), Unique]
        public string Slug { get; set; } // e.g. carbonmade

        [Member(3, Mutable = true)]
        public long RequestCount { get; set; }

        [Exclude] // current processes
        public IList<ProcessInfo> Processes { get; set; }
    }

    // - spawns one or more processes to handle requests (autoscales)
    // - hosted behind a load balancer

    // bindings (host, protocal, port)
}
