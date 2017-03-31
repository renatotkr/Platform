using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;
    using Net;

    [Dataset("NetworkProxies")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkProxy : INetworkProxy, ICloudResource
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

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #region IResource
        
        // aws
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        // arn:aws:elasticloadbalancing:us-west-2:123456789012:loadbalancer/app/my-load-balancer/50dc6c495c0c9188
        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType ICloudResource.Type => ResourceType.NetworkProxy;

        #endregion
    }
}
