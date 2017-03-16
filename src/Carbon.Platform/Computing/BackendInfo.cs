using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Platform.Computing
{
    [Dataset("Backends")]
    [DataIndex(IndexFlags.Unique, "providerId", "name")]
    public class BackendInfo
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("providerId")]
        public int ProviderId { get; set; }

        [Member("name")]
        public string Name { get; set; }

        [Member("locationId")]
        public long LocationId { get; set; }
        
        [Member("description")]
        public string Description { get; set; }

        // An internal or external facing proxy to access the backend
        
        [Member("proxyId")]
        public long? ProxyId { get; set; }

        [Member("healthCheck"), Optional]
        public HealthCheck HealthCheck { get; set; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }
        
        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; set; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }
    }
    
    public class HealthCheck
    {
        [DataMember(Name = "url")]
        public Uri Url { get; set; } // https://localhost:80

        [DataMember(Name = "interval")]
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(30);

        [DataMember(Name = "timeout")]
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);
        
        // HealthyThreshold : 4/5

        // UnhealthyThreshold : 5/5
    }
}


// AKA Auto-scaler / TargetGroup | Fleet

// arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067

// An webapp backend exposes an app/function as a load balanced worker or web service
// A worker backend manages one or more instances to handle a queue