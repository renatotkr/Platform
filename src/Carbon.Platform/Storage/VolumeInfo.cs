using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Storage
{
    using Data.Annotations;

    [Dataset("Volumes")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class VolumeInfo : IVolume
    {
        public VolumeInfo() { }

        public VolumeInfo(long id, long size, ManagedResource resource)
        {
            Id         = id;
            Size       = size;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("size")]
        public long Size { get; }

        [Member("hostId"), Mutable]
        public long? HostId { get; set; }

        // SourceImageId?

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
        public long LocationId { get; }

        ResourceType IManagedResource.ResourceType => ResourceType.Volume;
      
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
}