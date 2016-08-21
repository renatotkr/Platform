using System.Collections.Generic;

namespace Carbon.Platform
{
    using Data.Annotations;
    
    [Record(TableName = "Backends")]
    public class BackendInfo
    {
        [Identity]
        public long Id { get; set; }
        
        [Unique]
        public string Slug { get; set; } // e.g. carbonmade

        [Mutable]
        public long RequestCount { get; set; }

        // Last Load

        [Exclude] // current processes
        public IList<ProcessInfo> Processes { get; set; }
    }


    // A backend is an autoscaling group of programs hosted behind a load balancer

    // A backend spawns one or more processes to handle requests

    // bindings (host, protocal, port)
}
