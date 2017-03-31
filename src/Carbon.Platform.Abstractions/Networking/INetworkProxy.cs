using Carbon.Net;

namespace Carbon.Platform.Networking
{
    public interface INetworkProxy
    {
        long Id { get; }

        ProxyRoles Roles { get; }

        string Address { get; }
       
        long LocationId { get; }

        int ProviderId { get; }
    }
}

// AWS: arn:aws:elasticloadbalancing:us-west-2:123456789012:loadbalancer/app/my-load-balancer/50dc6c495c0c9188


// AKA Network virtual appliances 

// Types of Proxies
// - Load Balancer
// - Application Gateway
// - Firewall