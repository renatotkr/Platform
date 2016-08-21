using System.Net;

namespace Carbon.Networking
{
    // 2001:0db8:3c4d:0015:0000:0000:1a2f:1a2b

    public class IpAddress
    {
        public long Id { get; set; }

        // 4  = IP4
        // 16 = IP6
        IPAddress Value { get; }

        // A subnet network divides your global network in to regional subnets, each with its own IPv4 prefix. 

        // AddressType Type { get; }

        // Region

        // Created

        // physical or virtual

        // Users?
    }

    // internal / external

    public enum AddressType
    {
        Global,     // anycast      many routes
        Public,     // unicast      1 route 
        Private
    }

    public enum Layer
    {
        Physical = 1,
        Datalink = 2,
        Internet = 3,
        EndToEnd = 4,
        Applications = 7
    }
}


/*
// leftmost 48 bits
// long RoutingPrefix { get; set; }

// next 16 bytes
// short SubnetId { get; set; }


// long InterfaceId { get; set; }

/*
 * Hybrid dual-stack IPv6/IPv4 implementations recognize a special class of addresses, the IPv4-mapped IPv6 addresses. These addresses consist of an 80-bit prefix of zeros, the next 16 bits are one, and the remaining, least-significant 32 bits contain the IPv4 address. These addresses are typically written with a 96-bit prefix in the standard IPv6 format, and the remaining 32 bits written in the customary dot-decimal notation of IPv4. For example, ::ffff:192.0.2.128 represents the IPv4 address 192.0.2.128. A deprecated format for IPv4-compatible IPv6 addresses is ::192.0.2.128.[57]
 */
