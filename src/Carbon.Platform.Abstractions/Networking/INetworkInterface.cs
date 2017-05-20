using System.Net;

using Carbon.Platform.Resources;
using Carbon.Net;

namespace Carbon.Platform.Networking
{
    public interface INetworkInterface : IManagedResource
    {
        MacAddress MacAddress { get; }

        IPAddress[] Addresses { get; }
    }
}

// Network interfaces may be attached to hosts, load balancers, db instances, etc.