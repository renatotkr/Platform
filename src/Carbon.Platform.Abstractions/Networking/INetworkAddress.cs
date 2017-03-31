using System.Net;

namespace Carbon.Platform.Networking
{
    public interface INetworkAddress
    {
        long Id { get; set; }
        
        IPAddress Address { get; set; }
    }
}

// Amazon: eipalloc-5723d13e
// Google: ulong                                  compute#address