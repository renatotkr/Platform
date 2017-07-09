using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public interface INetworkRouter : IResource
    {
    }
}

// BGP (Peers / ASN)

// Each AWS VPC has an implict router that comes with a default route table.

// gcp | ? | compute#router