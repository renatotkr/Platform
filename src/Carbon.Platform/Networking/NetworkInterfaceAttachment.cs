using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkInterfaceAttachments")]
    public class NetworkInterfaceAttachment : INetworkInterfaceAttachment
    {
        public NetworkInterfaceAttachment(
            long networkInterfaceId, 
            long hostId, 
            DateTime created, 
            DateTime? deleted = null)
        {
            NetworkInterfaceId = networkInterfaceId;
            HostId = hostId;
            Created = created;
            Deleted = deleted;
        }

        [Member("networkInterfaceId"), Key]
        public long NetworkInterfaceId { get; }

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
