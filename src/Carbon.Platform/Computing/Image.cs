using System;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Json;

    // Images are immutable

    [Dataset("Images")]
    public class Image : ICloudResource
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("name")]
        [StringLength(50)] // e.g. Windows 2016 / 2016/01/01
        public string Name { get; set; }

        // { platform, hypervisor, architecture, ... }
        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        [Member("resourceName"), Unique]
        [Ascii, StringLength(100)]
        public string ResourceName { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        #region IPlatformResource

        ResourceType ICloudResource.Type => ResourceType.Image;

        CloudProvider ICloudResource.Provider => CloudProvider.Parse(ResourceName.Split(':')[0]);

        #endregion
    }

    // A platform encompass its underlying OS.
    // Platform: OS + Architecture

    // Windows/x86-64

    // AWS: ami

    public enum ImageStatus
    {
        Pending   = 0,
        Available = 1,
        Deleted   = 3
    }

    // machine | kernel | ramdisk
    /*
    public enum ImageType
    {
        OS = 1,
        Container
    }
    */
}
