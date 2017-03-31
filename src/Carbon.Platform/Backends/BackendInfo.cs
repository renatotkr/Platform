using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Computing
{
    [Dataset("Backends")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class BackendInfo : IBackend, ICloudResource
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; set; }
        
        [Member("locationId")]
        public long LocationId { get; set; }

        [Member("proxyId")]
        public long? ProxyId { get; set; }

        // Used to check the health of the backend instances
        [Member("healthCheckId")]
        public long? HealthCheckId { get; set; }

        // A template used to create new hosts for the backend
        [Member("hostTemplateId")]
        public long? HostTemplateId { get; set; }

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get;  }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }
        
        #region IResource

        // aws
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType ICloudResource.Type => ResourceType.Backend; 

        #endregion
    }
}

// An internal or external facing proxy to access the backend

// AWS    : TargetGroup for Application Load Balancer
// Google : Instance Group for Load Balanced Backend

// AKA: Fleet, Cluster
    

// An webapp backend exposes an app/function as a load balanced worker or web service
// A worker backend manages one or more instances to handle a queue