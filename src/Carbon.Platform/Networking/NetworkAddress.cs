using System;
using System.Net;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkAddresses")]
    [UniqueIndex("providerId", "resourceId")]
    public class NetworkAddress : INetworkAddress
    {
        public NetworkAddress() { }

        public NetworkAddress(
            long id, 
            IPAddress address, 
            ManagedResource resource, 
            long? hostId = null, 
            long? networkInterfaceId = null)
        {
            Id                 = id;
            Address            = address ?? throw new ArgumentNullException(nameof(address));
            ProviderId         = resource.ProviderId;
            ResourceId         = resource.ResourceId;
            HostId             = hostId;
            NetworkInterfaceId = networkInterfaceId;
        }

        // networkId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("address")]
        public IPAddress Address { get; }

        [Member("hostId"), Indexed]
        public long? HostId { get; }

        [Member("networkInterfaceId"), Indexed]
        public long? NetworkInterfaceId { get; }

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.NetworkAddress;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}