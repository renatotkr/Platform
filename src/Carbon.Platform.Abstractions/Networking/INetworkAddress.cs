using System.Net;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public interface INetworkAddress : IManagedResource
    {        
        IPAddress Address { get; }
    }
}

// Amazon: eipalloc-5723d13e
// Google: ulong                                  compute#address