namespace Carbon.Computing
{
    public enum IPAddressType
    {
        Anycast = 1, // many routes
        Unicast = 2, // 1 route 
        Private = 3
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
