using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;
    using Net;

    // Consider renaming networkAppliance

    [Dataset("NetworkProxies")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkProxy : INetworkProxy, IManagedResource
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("layer")]
        public NetworkLayer Layer { get; set; }

        [Member("roles")]
        public ProxyRoles Roles { get; set; }
        
        [Member("locationId")]
        public long LocationId { get; set; }

        [Member("networkId")]
        public long? NetworkId { get; set; }

        // Google : Unicast IP Address
        // AWS    : CNAME (name-424835706.us-west-2.elb.amazonaws.com)
        [Member("address")]
        public string Address { get; set; }

        #region IResource
        
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType IManagedResource.Type => ResourceType.NetworkProxy;

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
