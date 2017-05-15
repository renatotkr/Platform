using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public interface ISubnet : IManagedResource
    {
        long NetworkId { get; }
        
        string CidrBlock { get; }
    }
}

/*
aws   | subnet-8360a9e7 | Subnet
azure |  
gcp   | ulong           | compute#subnetwork
*/


// gcp : IpCidrRange
// azure  : Address Prefix

// e.g. 
// 10.1.1.0/24
// 2001:db8:1234:1a00::/64