using System.Net;

namespace Carbon.Platform.Networking
{
    public interface INetworkRoute
    {
        long Id { get; }

        long NetworkId { get; }

        string DestinationRange { get; }
        
        IPAddress NextHop { get; } // Specific Address or Host, Network, Gateway
        
        // Metric / Cost?,
        int Priority { get; }
    }

    public enum NextHopType
    {               // Azure                    Google
        None,       // None
        Gateway,    // VirtualNetworkGateway    Gatway
        Host,       // VirtualAppliance         Instance
        Network,    // VnetLocal                Network
        Internet,   // Internet
        Tunnel,     // ?                        nextHopVpnTunnel
    }
}


// The destination CIDR to which the route applies
// e.g. 10.1.0.0/16

// AKA
// Azure = Address Prefix
// Google = destRange

// e.g. 10.0.0.0/16

/*
Google : compute#route
*/

/*
-- The list of network routes compose a routing table / or RIB (routing information base)
     
NAME                           NETWORK   DEST_RANGE    NEXT_HOP                 PRIORITY
default-route-02a98b9a14f7edc4 default   10.128.0.0/20                          1000
default-route-081fa300345dd52a default   0.0.0.0/0     default-internet-gateway 1000
default-route-93a38d78c77eac66 default   10.132.0.0/20                          1000
default-route-999664b72dd247e7 default   10.140.0.0/20                          1000
default-route-a1f15d0858cd51e1 default   10.142.0.0/20                          1000
*/
