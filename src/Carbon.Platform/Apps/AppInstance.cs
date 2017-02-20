using System;

namespace Carbon.Platform.Apps
{
    using Computing;
    using Data.Annotations;
    using Json;
    using Versioning;
    
    // Compound index (host + status) ?

    [Dataset("AppInstances")]
    public class AppInstance : IApp
    {
        public AppInstance() { }

        public AppInstance(IApp app, IHost host)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (host == null)
                throw new ArgumentNullException(nameof(host));

            #endregion

            AppId = app.Id;
            AppName = app.Name;
            AppVersion = app.Version;
            HostId = host.Id;
        }

        [Member("appId"), Key]
        public long AppId { get; set; }

        [Member("hostId"), Key]
        [Indexed]
        public long HostId { get; set; }

        [Member("env")]
        [StringLength(1000)]
        public JsonObject Env { get; }

        [Member("status"), Mutable]
        public HostStatus Status { get; set; }

        [Member("appName")]
        [StringLength(50)]
        public string AppName { get; set; }
        
        [Member("appVersion"), Mutable] 
        public SemanticVersion AppVersion { get; set; }

        [Member("heartbeat"), Mutable]
        public DateTime? Heartbeat { get; set; }
        
        [Member("port")]
        public int? Port { get; set; }

        // LoadBalancerId? 

        [Member("requestCount"), Mutable]
        public long RequestCount { get; set; }

        [Member("errorCount"), Mutable]
        public long ErrorCount { get; set; }

        [Member("terminated"), Mutable]
        public DateTime? Terminated { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        #region IApp

        long IApp.Id => AppId;

        SemanticVersion IApp.Version => AppVersion;

        string IApp.Name => AppName;

        #endregion
    }    
}