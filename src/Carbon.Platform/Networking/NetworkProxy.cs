using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;
    using Net;

    [Dataset("NetworkProxies")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkProxy : INetworkProxy
    {
        public NetworkProxy() { }

        public NetworkProxy(long id, ProxyType type, ManagedResource resource)
        {
            Id         = id;
            Type       = type;
            ProviderId = resource.Provider.Id;
            LocationId = resource.LocationId;
            ResourceId = resource.Id;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("type")]
        public ProxyType Type { get; }

        // Google : Unicast IP Address
        // AWS    : CNAME (name-424835706.us-west-2.elb.amazonaws.com)
        [Member("address")]
        public string Address { get; set; }

        public long NetworkId => ScopedId.GetScope(Id);

        #region IResource
        
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public long LocationId { get; set; }

        ResourceType IManagedResource.ResourceType => ResourceType.NetworkProxy;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }

    // Type = LoadBalancer | Firewall
}
