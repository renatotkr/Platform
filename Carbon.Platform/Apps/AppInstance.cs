using System;

namespace Carbon.Computing
{
    using Data.Annotations;

    [Dataset("AppInstances")]
    [Index(IndexFlags.Unique, new[] { "appId", "hostId" })]
    public class AppInstance
    {        
        [Member(1)]
        public long AppId { get; set; }

        [Member(2)]
        public long HostId { get; set; }

        [Member(3), Mutable]
        public Semver AppVersion { get; set; }

        // Current process?

        [Member(5)]
        public DateTime? Terminated { get; set; }

    }
}

// - responsible for monitoring the health of the processes (taking them into and out of service as needed)
