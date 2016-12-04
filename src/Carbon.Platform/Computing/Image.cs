using System;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;

    [Dataset("Images")]
    public class Image
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("name")]
        [StringLength(50)] // e.g. Windows 2016 / 2016/01/01
        public string Name { get; set; }

        [Member("description")]
        [StringLength(100)] // Ubuntu Server 8.04 (Hardy Heron)
        public string Description { get; set; }

        // [Member("os")]  // Windows/2016, Linux/4.34
        // public string Platform { get; set; }

        // i386 | x86_64 / ARM
        [Member("architecture")]
        public string Architecture { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        [Member("refId"), Unique] // Provider Specific Id
        [Ascii, StringLength(50)]
        public string RefId { get; set; }

        // hypervisor
        // OS / Platform
    }

    // A platform encompass its underlying OS.
    // Platform: OS + Architecture
    
    // AWS: ami

    public enum ImageStatus
    {
        Pending = 0,
        Available = 1
    }

    // machine | kernel | ramdisk
    public enum ImageType
    {
        OS = 1,
        Container
    }
}
