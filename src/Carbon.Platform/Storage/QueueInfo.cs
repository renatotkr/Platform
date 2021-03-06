﻿using System;

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
            long ownerId,
            string name,
            ManagedResource resource,
            JsonObject properties = null)
        {
            Ensure.IsValidId(id);
            Ensure.IsValidId(ownerId, nameof(ownerId));
            Ensure.NotNullOrEmpty(name, nameof(name));

            Id         = id;
            Name       = name;
            OwnerId    = ownerId;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
            Properties = properties;
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

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("resourceId")]
        [StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.Queue;

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