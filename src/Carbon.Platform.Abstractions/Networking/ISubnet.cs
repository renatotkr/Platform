using System.Net;

namespace Carbon.Platform.Networking
{
    public interface ISubnet
    {
        long Id { get; }

        long NetworkId { get; }

        // google: ipCidrRange
        string CidrBlock { get; }

        IPAddress GatewayAddress { get; }

        long LocationId { get; }
    }
}



/*
Amazon      subnet-8360a9e7     Subnet
Azure   
Google      ulong               compute#subnetwork
*/

// Details
// -gatewayAddress

/*
us-west1	    10.138.0.0/20	10.138.0.1
us-central1	    10.128.0.0/20	10.128.0.1
us-east1	    10.142.0.0/20	10.142.0.1
europe-west1	10.132.0.0/20	10.132.0.1
asia-east1	    10.140.0.0/20	10.140.0.1
*/
