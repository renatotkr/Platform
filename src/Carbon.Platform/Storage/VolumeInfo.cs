using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Storage
{
    using Data.Annotations;

    [Dataset("Volumes")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class VolumeInfo : IVolume, IVolumeStats, IManagedResource
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("status"), Mutable]
        public VolumeStatus Status { get; set; }

        [Member("size")]
        public long Size { get; set; }

        [Member("locationId")]
        public long LocationId { get; set; }

        [Member("hostId"), Mutable]
        public long? HostId { get; set; }

        // SourceImageId?

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(63)]
        public string ResourceId { get; set; }

        ResourceType IManagedResource.Type => ResourceType.Volume;
      
        #endregion

        #region Stats / Health

        [Member("heartbeat")]
        public DateTime? Heartbeat { get; set; }

        [Member("bytesRead")]
        public long BytesRead { get; set; }

        [Member("bytesWritten")]
        public long BytesWritten { get; set; }

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