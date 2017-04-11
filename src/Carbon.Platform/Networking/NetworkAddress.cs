using System;
using System.Net;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkAddresses")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkAddress : INetworkAddress
    {
        public NetworkAddress() { }

        public NetworkAddress(long id, ManagedResource resource)
        {
            Id = id;
            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("networkInterfaceId")]
        [Indexed]
        public long? NetworkInterfaceId { get; set; }

        [Member("address")]
        public IPAddress Address { get; set; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        long IManagedResource.LocationId => 0;

        ResourceType IManagedResource.ResourceType => ResourceType.NetworkAddress;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        [IgnoreDataMember]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #endregion
    }
}