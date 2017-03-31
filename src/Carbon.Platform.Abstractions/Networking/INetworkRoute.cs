using System.Net;
using Carbon.Net;

namespace Carbon.Platform.Networking
{
    public interface INetworkRoute
    {
        long Id { get; }

        long NetworkId { get; }

        // e.g. 192.168.0.0/2
        Cidr DestinationRange { get; }  // Network Destiation + Netmask

        // aka. NextHop
        IPAddress Gateway { get; }
        
        IPAddress Interface { get; }

        // Metric / Cost?
        int Priority { get; }
    }
    
    // The list of network routes compose a routing table / or RIB (routing information base)
}