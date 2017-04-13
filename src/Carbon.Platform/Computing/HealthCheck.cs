using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Net;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("HealthChecks")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class HealthCheck : IHealthCheck, IManagedResource
    {
        public HealthCheck() { }

        public HealthCheck(
            long id,
            string host,
            string path,
            ushort port,
            NetworkProtocal protocal)
        {
            Id       = id;
            Host     = host;
            Path     = path;
            Port     = port;
            Protocal = protocal;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("host"), Optional]
        [StringLength(100)]
        public string Host { get; }

        [Member("path")]
        [StringLength(100)]
        public string Path { get; }

        [Member("port")]
        public ushort Port { get; }

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

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        [IgnoreDataMember]
        [Member("locationId")]
        public int LocationId { get; set; }

        ResourceType IResource.ResourceType => ResourceType.HealthCheck;

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