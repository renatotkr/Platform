using System.Net;
using Carbon.Net;

namespace Carbon.Platform.Networking
{
    public interface INetworkRoute
    {
        long Id { get; }

        long NetworkId { get; }

        // The destination CIDR to which the route applies
        // e.g. 10.1.0.0/16

        // AKA
        // Azure = Address Prefix
        // Google = destRange

        Cidr DestinationRange { get; }
        
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



    // The list of network routes compose a routing table / or RIB (routing information base)
}


/*
Google : compute#route
*/
