using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Storage
{
    [Dataset("VolumeAttachments")]
    public class VolumeAttachment : IVolumeAttachment
    {
        public VolumeAttachment(long volumeId, long hostId, DateTime created, DateTime? deleted = null)
        {
            VolumeId = volumeId;
            HostId = hostId;
            Created = created;
            Deleted = deleted;
        }

        [Member("volumeId"), Key]
        public long VolumeId { get; }

        [Member("hostId"), Key]
        public long HostId { get; }

        [Member("created")]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }
    }
}
