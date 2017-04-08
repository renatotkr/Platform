using System;
using System.Runtime.Serialization;

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

        public AppInstance(IApp app, SemanticVersion version, IBackend backend,  IHost host)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));
            
            if (backend == null)
                throw new ArgumentNullException(nameof(backend));

            if (host == null)
                throw new ArgumentNullException(nameof(host));
            
            #endregion

            AppId         = app.Id;
            AppName       = app.Name;
            AppVersion    = version;
            BackendId     = backend.Id;
            EnvironmentId = backend.EnvironmentId;
            HostId        = host.Id;
        }

        [Member("appId"), Key]
        public long AppId { get; }

        [Member("hostId"), Key]
        [Indexed]
        public long HostId { get; }

        [Member("status"), Mutable]
        public HostStatus Status { get; set; }

        [Member("appName")]
        [StringLength(50)]
        public string AppName { get; }
        
        [Member("appVersion"), Mutable] 
        public SemanticVersion AppVersion { get; set; }
        
        [Member("backendId")]
        [Indexed]
        public long? BackendId { get; }

        [Member("environmentId")]
        [Indexed]
        public long EnvironmentId { get; set; }

        [Member("port")]
        public int? Port { get; set; }

        #region Health & Stats

        [Member("heartbeat")]
        public DateTime? Heartbeat { get; set; }

        [Member("requestCount")]
        public long RequestCount { get; set; }

        [Member("errorCount")]
        public long ErrorCount { get; set; }

        #endregion

        #region Timestamps

        [Member("terminated"), Mutable]
        public DateTime? Terminated { get; set; }

        // Modified

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        #endregion

        #region Helpers

        [IgnoreDataMember]
        public bool IsTerminated => Terminated != null;

        #endregion

        #region IApp

        long IApp.Id => AppId;

        string IApp.Name => AppName;

        #endregion
    }    
}