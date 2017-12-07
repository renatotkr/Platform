using System;
using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Rds
{
    [Dataset("DatabaseClusters", Schema = "Rds")]
    public class DatabaseCluster : IDatabaseCluster
    {
        public DatabaseCluster() { }

        public DatabaseCluster(long id, string name, ManagedResource resource, JsonObject properties = null)
        {
            Validate.Id(id, nameof(id));
            Validate.NotNullOrEmpty(name, nameof(name));

            Id         = id;
            Name       = name;
            ResourceId = resource.ResourceId;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            Properties = properties ?? new JsonObject();
        }

        // databaseId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }
        
        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        public long DatabaseId => ScopedId.GetScope(Id);

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("resourceId")]
        [StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.DatabaseCluster;

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
