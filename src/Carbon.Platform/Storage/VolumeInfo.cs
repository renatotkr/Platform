using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    [Dataset("Volumes", Schema = "Storage")]
    [UniqueIndex("providerId", "resourceId")]
    public class VolumeInfo : IVolume
    {
        public VolumeInfo() { }

        public VolumeInfo(
            long id, 
            long ownerId,
            long size,
            ManagedResource resource,
            JsonObject properties = null,
            long? hostId = null)
        {
            Ensure.IsValidId(id);
            Ensure.IsValidId(ownerId, nameof(ownerId));

            Id         = id;
            OwnerId    = ownerId;
            Size       = size;
            HostId     = hostId;
            Properties = properties;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
        }

        [Member("id"), Key(sequenceName: "volumeId")]
        public long Id { get; }

        [Member("ownerId"), Indexed]
        public long OwnerId { get; }

        [Member("size")] // in bytes
        public long Size { get; }

        [Member("hostId"), Indexed]
        public long? HostId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(63)]
        public string ResourceId { get; }

        [IgnoreDataMember]
        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.Volume;
      
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