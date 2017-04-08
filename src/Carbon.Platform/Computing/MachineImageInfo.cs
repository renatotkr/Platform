using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;

    [Dataset("MachineImages")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class MachineImageInfo : IMachineImage
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; set; }

        [Member("description")]
        [StringLength(100)]
        public string Description { get; set; }

        [Member("type")]
        public MachineImageType ImageType { get; set; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        long IManagedResource.LocationId => 0;

        ResourceType IManagedResource.ResourceType => ResourceType.MachineImage;

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