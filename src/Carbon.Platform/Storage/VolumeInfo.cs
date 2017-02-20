using System;

namespace Carbon.Platform.Storage
{
    using Data.Annotations;
    using Json;

    [Dataset("Volumes")]
    public class VolumeInfo : ICloudResource
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("status"), Mutable]
        public VolumeStatus Status { get; set; }

        [Member("size")]
        public long Size { get; set; } // in octets

        [Member("hostId"), Mutable]
        public long? HostId { get; set; }

        [Member("resourceName"), Unique]
        [Ascii, StringLength(100)]
        public string ResourceName { get; set; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        [Member("modified"), Timestamp]
        public DateTime Modified { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        #region IPlatformResource

        ResourceType ICloudResource.Type => ResourceType.Volume;

        CloudPlatformProvider ICloudResource.Provider => 
            CloudPlatformProvider.Parse(ResourceName.Split(':')[0]);

        #endregion
    }
}