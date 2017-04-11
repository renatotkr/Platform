namespace Carbon.Platform.Networking
{
    public interface INetworkRouter : IManagedResource
    {
        long Id { get; }

        long NetworkId { get; }
    }
}

// BGP (Peers / ASN)

// Each AWS VPC has an implict router that comes with a default route table.

// Google: compute#router