using System.Net;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public interface INetwork : IManagedResource
    {
        IPAddress GatewayAddress { get; }

        // e.g. 192.168.0.0/16
        string CidrBlock { get; }

        // VPC's may have seperate IPv4 & IPv6 ranges
        // ip4range
        // ip6range
    }
}

// A network may span multiple locations

// Amazon | VPC
// Google | compute#network

/*
us-west1	    10.138.0.0/20	10.138.0.1
us-central1	    10.128.0.0/20	10.128.0.1
us-east1	    10.142.0.0/20	10.142.0.1
europe-west1	10.132.0.0/20	10.132.0.1
asia-east1	    10.140.0.0/20	10.140.0.1
*/
