using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
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
            long size,
            long ownerId,
            ManagedResource resource,
            long? hostId = null)
        {
            Id         = id;
            Size       = size;
            OwnerId    = ownerId;
            HostId     = hostId;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
        }

        [Member("id"), Key(sequenceName: "volumeId")]
        public long Id { get; }

        [Member("size")] // in bytes
        public long Size { get; }

        [Member("hostId")]
        public long? HostId { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("flags")]
        public VolumeFlags Flags { get; set; }

        // IOPS
        // ...

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
    
    public enum VolumeFlags
    {
        None = 0,
        SDD  = 1 << 0
    }
}