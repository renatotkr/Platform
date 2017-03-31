namespace Carbon.Platform.Networking
{
    using Net;

    public interface INetworkProxy
    {
        long Id { get; }

        ProxyRoles Roles { get; }

        string Address { get; }
       
        long LocationId { get; }

        int ProviderId { get; }
    }
}

// Types of Proxies
// - Load Balancer
// - Application Gateway
// - Firewall