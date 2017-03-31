using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Json;

    [Dataset("Images")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class ImageInfo : IMachineImage, ICloudResource
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
        public ImageType Type { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        // Google: 6864121747226459524
        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType ICloudResource.Type => ResourceType.MachineImage;

        #endregion
    }
}