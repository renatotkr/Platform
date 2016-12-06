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
            AppId = app.Id;
            AppName = app.Name;
            HostId = host.Id;
        }

        [Member("appId"), Key]
        public long AppId { get; set; }

        [Member("hostId"), Key]
        [Indexed]
        public long HostId { get; set; }


        [Member("env")]
        public JsonObject Env { get; }

        [Member("status"), Mutable]
        public AppStatus Status { get; set; }

        [Member("appName")]
        [StringLength(50)]
        public string AppName { get; set; }
        
        [Member("appVersion"), Mutable] 
        public SemanticVersion AppVersion { get; set; }

        // Started

        // Health?

        [Member("terminated")]
        public DateTime? Terminated { get; set; }

        [Member("heartbeat")]
        public DateTime? Heartbeat { get; set; }

        
        // requestCount ?


        // Port

        [Member("created")]
        public DateTime Created { get; set; }

        #region IApp

        long IApp.Id => AppId;

        SemanticVersion IApp.Version => AppVersion;

        string IApp.Name => AppName;

        #endregion
    }

    
}