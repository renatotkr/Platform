using System;

using Carbon.Data.Annotations;
using Carbon.Net;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("HealthChecks", Schema = "Computing")]
    [UniqueIndex("providerId", "resourceId")]
    public class HealthCheck : IHealthCheck, IManagedResource
    {
        public HealthCheck() { }

        public HealthCheck(
           long id,  
           long ownerId,
           Uri url,
           ManagedResource resource)
            : this(id, ownerId, url.Host, url.AbsolutePath, url.Port, NetworkProtocal.TCP, resource) { }

        public HealthCheck(
            long id, 
            long ownerId,
            string host,
            string path,
            int port,
            NetworkProtocal protocal,
            ManagedResource resource)
        {
            Ensure.IsValidId(id);
            Ensure.IsValidId(ownerId, nameof(ownerId));
            
            Id         = id;
            OwnerId    = ownerId;
            Host       = host;
            Path       = path;
            Port       = port;
            Protocal   = protocal;
            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
            LocationId = resource.LocationId;
        }
        
        [Member("id"), Key(sequenceName: "healthCheckId")] // environmentId | #
        public long Id { get; }

        [Member("ownerId"), Indexed]
        public long OwnerId { get; }

        [Member("host"), Optional]
        [Ascii, StringLength(253)]
        public string Host { get; }

        [Member("path"), Optional]
        [StringLength(100)]
        public string Path { get; }

        [Member("port")]
        public int Port { get; }

        [Member("protocal")] // e.g. TCP, HTTP, HTTPS
        public NetworkProtocal Protocal { get; }
        
        [Member("interval")]
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(30);

        [Member("timeout")]
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);
        
        [Member("healthyThreshold")]
        public int HealthyThreshold { get; set; }

        [Member("unhealtyThreshold")]
        public int UnhealthyThreshold { get; set; }       

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.HealthCheck;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}