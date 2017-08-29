using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("Clusters", Schema = "Computing")]
    [UniqueIndex("environmentId", "locationId")]
    public class Cluster : ICluster
    {
        public Cluster() { }

        public Cluster(
            long id,
            string name,
            long environmentId,
            int locationId,
            long hostTemplateId,
            JsonObject properties = null,
            long? healthCheckId = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (name == null || string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            if (locationId <= 0)
                throw new ArgumentException("Must be > 0", nameof(locationId));

            #endregion

            Id             = id;
            Name           = name;
            EnvironmentId  = environmentId;
            Properties     = properties ?? new JsonObject();
            LocationId     = locationId;
            HostTemplateId = hostTemplateId;
        }
        
        [Member("id"), Key("clusterId")]
        public long Id { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        [Member("environmentId")]
        public long EnvironmentId { get; }
        
        [Member("locationId")]  // global, regional, or zonal
        public int LocationId { get; }

        [Member("hostTemplateId")]
        public long HostTemplateId { get; }
        
        [Member("healthCheckId")]
        public long? HealthCheckId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Cluster;

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

    public static class ClusterProperties
    {
        // When used with an application load balancer target group
        public const string TargetGroupArn = "targetGroupArn";
    }
}