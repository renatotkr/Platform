using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Sequences;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Data
{
    [Dataset("DatabaseClusters")]
    public class DatabaseCluster : IDatabaseCluster
    {
        public DatabaseCluster() { }

        public DatabaseCluster(long id, string name, ManagedResource resource)
        {
            Id = id;
            Name = name;

            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
        }

        // databaseId + Sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        // TODO: Endpoints

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [StringLength(100)]
        public string ResourceId { get; }

        [IgnoreDataMember]
        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceType.DatabaseCluster;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion

        public long DatabaseId => ScopedId.GetScope(Id);
    }
}
