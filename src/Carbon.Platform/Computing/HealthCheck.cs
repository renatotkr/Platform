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
           Uri url,
           long ownerId,
           ManagedResource resource)
            : this(id, url.Host, url.AbsolutePath, url.Port, NetworkProtocal.TCP, ownerId, resource) { }

        public HealthCheck(
            long id,
            string host,
            string path,
            int port,
            NetworkProtocal protocal,
            long ownerId,
            ManagedResource resource)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

            Id = id;
            Host       = host;
            Path       = path;
            Port       = port;
            Protocal   = protocal;
            OwnerId    = ownerId;
            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
            LocationId = resource.LocationId;
        }
        
        [Member("id"), Key(sequenceName: "healthCheckId")]
        public long Id { get; }

        [Member("host"), Optional]
        [StringLength(100)]
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

        [Member("ownerId"), Indexed]
        public long OwnerId { get; }

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