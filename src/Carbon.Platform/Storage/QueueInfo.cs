using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    [Dataset("Queues", Schema = "Storage")]
    [UniqueIndex("ownerId", "name")]
    public class QueueInfo : IQueueInfo
    {
        public QueueInfo() { }

        public QueueInfo(
            long id, 
            string name,
            long ownerId,
            ManagedResource resource,
            JsonObject properties = null)
        {
            Id         = id;
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            OwnerId    = ownerId;
            Properties = properties;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
        }

        [Member("id"), Key(sequenceName: "queueId")]
        public long Id { get; }

        [Member("ownerId")]
        public long OwnerId { get; }
        
        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

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

        ResourceType IResource.ResourceType => ResourceTypes.Queue;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}