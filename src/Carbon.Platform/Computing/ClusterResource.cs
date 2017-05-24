using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("ClusterResources")]
    [UniqueIndex("clusterId", "resourceType", "resourceId")]
    public class ClusterResource
    {
        public ClusterResource() { }

        public ClusterResource(
            long id,
            ICluster cluster,
            IResource resource,
            long? environmentId = null,
            ILocation location  = null
        )
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (cluster == null)
                throw new ArgumentNullException(nameof(cluster));
  
            if (resource == null)
                throw new ArgumentNullException(nameof(resource));

            #endregion

            Id            = id;
            ClusterId     = cluster.Id;
            ResourceType  = resource.ResourceType.Name;
            ResourceId    = resource.Id;
            EnvironmentId = environmentId;
            LocationId    = location?.Id;
        }

        // clusterId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("clusterId")]
        public long ClusterId { get; }

        // e.g. host
        [Member("resourceType")]
        [StringLength(30)]
        public string ResourceType { get; }

        // e.g. 1
        [Member("resourceId")]
        public long ResourceId { get; }

        // Shortcuts...
        [Member("environmentId")]
        [Indexed]
        public long? EnvironmentId { get; }

        [Member("locationId")]
        public int? LocationId { get; }
    }

    // A cluster may span mutiple environments & locations
    // and include storage, compute, and network resources
}