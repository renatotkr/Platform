using System;

namespace Carbon.Platform.Apps
{
    using Data.Annotations;
    using Versioning;

    [Dataset("AppInstances")]
    public class AppInstance
    {        
        [Member("appId"), Key]
        public long AppId { get; set; }

        [Member("hostId"), Key]
        public long HostId { get; set; }

        [Member("status")]
        public AppInstanceStatus Status { get; set; }

        [Member("appVersion"), Mutable] 
        public SemanticVersion AppVersion { get; set; }

        [Member("terminated")]
        public DateTime? Terminated { get; set; }

        [Member("heartbeat")]
        public DateTime? Heartbeat { get; set; }

        // requestCount ?

        // Port

        [Member("created")]
        public DateTime Created { get; set; }
    }

    public enum AppInstanceStatus
    {
        Pending     = 0, // provisioning
        Running     = 1,
        Suspending  = 2, // stopping
        Suspended   = 3, // stopped
        Terminating = 4, // shutting down ?
        Terminated  = 5
    }
}