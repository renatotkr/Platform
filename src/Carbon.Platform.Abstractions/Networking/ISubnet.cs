using System.Net;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public interface ISubnet : IManagedResource
    {
        long NetworkId { get; }
        
        string CidrBlock { get; }

        IPAddress GatewayAddress { get; }
    }
}

/*
Amazon      subnet-8360a9e7     Subnet
Azure   
Google      ulong               compute#subnetwork
*/


// Google : IpCidrRange
// Azure  : Address Prefix

// e.g. 
// 10.1.1.0/24
// 2001:db8:1234:1a00::/64