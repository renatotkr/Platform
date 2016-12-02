using System;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;

    [Dataset("Images")]
    public class Image
    {
        [Member("id"), Identity]
        public long Id { get; }

        [Member("name")]
        [StringLength(100)] // e.g. Windows 2016 / 2016/01/01
        public string Name { get; set; }

        // i386 | x86_64
        // [Column]
        // public string Architecture { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        [Member("refId"), Indexed] // Provider Specific Id
        [Ascii, StringLength(50)]
        public string RefId { get; set; }

        // OS / Platform
    }

    public enum ImageStatus
    {
        Pending = 0,
        Available = 1
    }

    public enum ImageType
    {
        OS = 1,
        Container
    }
}
