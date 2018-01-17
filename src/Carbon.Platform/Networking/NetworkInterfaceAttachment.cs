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
            Ensure.IsValidId(networkInterfaceId, nameof(networkInterfaceId));
            Ensure.IsValidId(hostId,             nameof(hostId));

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
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }
    }
}
