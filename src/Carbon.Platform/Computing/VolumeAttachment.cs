using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Computing
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
        [TimePrecision(TimePrecision.Second)]
        public DateTime Created { get; }

        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }
    }
}
