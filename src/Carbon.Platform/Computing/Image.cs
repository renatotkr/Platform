using System;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Json;

    // Images are immutable

    [Dataset("Images")]
    public class Image
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("name")]
        [StringLength(50)] // e.g. Windows 2016 / 2016/01/01
        public string Name { get; set; }

        // { platform, architecture, ... }
        [Member("details")]
        public JsonObject Details { get; set; }

        [Member("refId"), Unique] // Provider Specific Id
        [Ascii, StringLength(50)]
        public string RefId { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        // hypervisor
        // OS / Platform
    }

    // A platform encompass its underlying OS.
    // Platform: OS + Architecture
    
    // AWS: ami

    public enum ImageStatus
    {
        Pending   = 0,
        Available = 1,
        Deleted   = 3
    }

    // machine | kernel | ramdisk
    public enum ImageType
    {
        OS = 1,
        Container
    }
}
