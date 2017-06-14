using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("Clusters", Schema = "Computing")]
    [UniqueIndex("providerId", "resourceId")]
    public class Cluster : ICluster
    {
        public Cluster() { }

        public Cluster(
            long id,
            string name,
            long environmentId,
            JsonObject properties,
            ManagedResource resource,
            long? hostTemplateId,
            long? healthCheckId)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id             = id;
            Name           = name ?? throw new ArgumentNullException(nameof(name));
            EnvironmentId  = environmentId;
            Properties     = properties ?? throw new ArgumentNullException(nameof(properties));
            ProviderId     = resource.ProviderId;
            LocationId     = resource.LocationId;
            ResourceId     = resource.ResourceId;
            HostTemplateId = hostTemplateId;
        }
        
        [Member("id"), Key("clusterId")]
        public long Id { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        [Member("environmentId")]
        [Indexed]
        public long? EnvironmentId { get; }

        [Member("hostTemplateId")]
        public long? HostTemplateId { get; }
        
        [Member("healthCheckId")]
        public long? HealthCheckId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; set; }

        #region Stats

        // The number of resources added to the cluster
        // e.g. buckets, encryptionKeys, loadBalancers, etc

        [Member("resourceCount")]
        public int ResourceCount { get; }

        #endregion

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }
        
        // global, regional, or zonal
        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.Cluster;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }

    public static class ClusterProperties
    {
        // When used with an application load balancer target group
        public const string TargetGroupArn = "targetGroupArn";
    }
}