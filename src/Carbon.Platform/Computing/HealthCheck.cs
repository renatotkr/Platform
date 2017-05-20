using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Net;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("HealthChecks", Schema = "Computing")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
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

        [Member("path")]
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
        public int HealthyThreshold { get; set; }     // e.g. 4/5

        [Member("unhealtyThreshold")]
        public int UnhealthyThreshold { get; set; }   // e.g. 5.5

        [Member("ownerId")]
        public long OwnerId { get; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [IgnoreDataMember]
        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.HealthCheck;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        #endregion
    }
}