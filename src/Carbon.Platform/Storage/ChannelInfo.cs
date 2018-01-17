using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    [Dataset("Channels")]
    [UniqueIndex("ownerId", "name")]
    public class ChannelInfo : IChannelInfo
    {
        public ChannelInfo() { }

        public ChannelInfo(
            long id,  
            long ownerId,
            string name,
            ManagedResource resource, 
            JsonObject properties = null)
        {
            Ensure.IsValidId(id);
            Ensure.IsValidId(ownerId,          nameof(ownerId));
            Ensure.NotNullOrEmpty(name, nameof(name));

            Id         = id;
            OwnerId    = ownerId;
            Name       = name;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
            Properties = properties;
        }

        [Member("id"), Key(sequenceName: "channelId")]
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

        ResourceType IResource.ResourceType => ResourceTypes.Channel;

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
