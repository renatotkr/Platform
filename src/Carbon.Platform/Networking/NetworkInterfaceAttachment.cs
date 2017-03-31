using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkInterfaceAttachments")]
    public class NetworkInterfaceAttachment : INetworkInterfaceAttachment
    {
        [Member("networkInterfaceId"), Key]
        public long NetworkInterfaceId { get; set; }

        [Member("hostId"), Key]
        public long HostId { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        [Member("deleted")]
        public DateTime? Deleted { get; set; }
    }
}
