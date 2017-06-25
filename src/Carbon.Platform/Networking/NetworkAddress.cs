using System;
using System.Net;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkAddresses")]
    [UniqueIndex("providerId", "resourceId")]
    public class NetworkAddress : INetworkAddress
    {
        public NetworkAddress() { }

        public NetworkAddress(long id, IPAddress address, ManagedResource resource)
        {
            Id         = id;
            Address    = address ?? throw new ArgumentNullException(nameof(address));
            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
        }

        // networkId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("address")]
        public IPAddress Address { get; }

        [Member("hostId")]
        [Indexed]
        public long? HostId { get; set; }

        [Member("networkInterfaceId")]
        [Indexed]
        public long? NetworkInterfaceId { get; set; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        int IManagedResource.LocationId => 0;

        ResourceType IResource.ResourceType => ResourceTypes.NetworkAddress;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        [IgnoreDataMember]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}