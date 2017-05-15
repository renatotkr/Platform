using System.Net;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public interface INetworkAddress : IManagedResource
    {        
        IPAddress Address { get; }
    }
}

// aws: eipalloc-5723d13e | ?
// gcp: ulong             | compute#address