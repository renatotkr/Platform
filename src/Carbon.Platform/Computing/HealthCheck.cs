using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Net;

namespace Carbon.Platform.Computing
{
    [Dataset("HealthChecks")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class HealthCheck : IHealthCheck, IManagedResource
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("path")]
        public string Path { get; set; }

        [Member("port")]
        public ushort Port { get; set; }

        [Member("host"), Optional]
        public string Host { get; set; }

        [Member("protocal")] // e.g. TCP, HTTP, HTTPS
        public NetworkProtocal Protocal { get; set; }
        
        [Member("interval")]
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(30);

        [Member("timeout")]
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);
        
        [Member("healthyThreshold")]
        public int HealthyThreshold { get; set; }     // e.g. 4/5

        [Member("unhealtyThreshold")]
        public int UnhealthyThreshold { get; set; }   // e.g. 5.5

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        long IManagedResource.LocationId => 0;

        ResourceType IManagedResource.ResourceType => ResourceType.HealthCheck;

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