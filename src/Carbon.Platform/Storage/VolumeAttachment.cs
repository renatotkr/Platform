using Carbon.Data.Annotations;
using System;

namespace Carbon.Platform.Storage
{
    [Dataset("VolumeAttachments")]
    public class VolumeAttachment : IVolumeAttachment
    {
        [Member("volumeId"), Key]
        public long VolumeId { get; set; }

        [Member("hostId"), Key]
        public long HostId { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        [Member("deleted")]
        public DateTime? Deleted { get; set; }
    }
}
