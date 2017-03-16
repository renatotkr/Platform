using System;
using System.Runtime.Serialization;
using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkProxies")]
    [DataIndex(IndexFlags.Unique, "providerId", "name")]
    public class NetworkProxy : ICloudResource
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("providerId")]
        public int ProviderId { get; set; }

        [Member("name")]
        [Ascii, StringLength(50)]
        public string Name { get; set; }

        [Member("networkId")]
        public long? NetworkId { get; set; }

        [Member("layer")]
        public NetworkLayer Layer { get; set; }

        [Member("roles")]
        public ProxyRoles Roles { get; set; }
        
        [Member("locationId")]
        public long LocationId { get; set; }

        // Google : Unicast IP Address
        // AWS    : CNAME (name-424835706.us-west-2.elb.amazonaws.com)
        [Member("address")]
        public string Address { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [Member("deleted")]
        public DateTime? Deleted { get; set; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }

        #region Helpers

        [IgnoreDataMember]
        public bool IsDeleted => Deleted != null;

        #endregion

        // Listeners

        #region IResource

        ResourceType ICloudResource.Type => ResourceType.NetworkProxy;

        [IgnoreDataMember]
        public CloudProvider Provider => CloudProvider.Get(ProviderId);

        #endregion 
    }

    // Addresses / 
    // Rules?

    [Flags]
    public enum ProxyRoles
    {
        LoadBalancer   = 1 << 0,
        Waf            = 1 << 1, // Enforces Network Rules
        IP6Termination = 1 << 2, // Terminates Ip6 traffic
        SslTermination = 1 << 3  // Terminates Ssl traffic
    }
}

//  A NetworkProxy / LoadBalancer may span mutiple availabity zones within a region

// e.g. 
// arn:aws:elasticloadbalancing:us-west-2:123456789012:loadbalancer/app/my-load-balancer/50dc6c495c0c9188
