using System.Net;

namespace Carbon.Platform.Networking
{
    public interface INetworkAddress : IManagedResource
    {
        long Id { get; }
        
        IPAddress Address { get; }
    }
}

// Amazon: eipalloc-5723d13e
// Google: ulong                                  compute#address