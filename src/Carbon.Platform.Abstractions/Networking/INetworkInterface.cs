using System.Net;

using Carbon.Platform.Resources;
using Carbon.Net;

namespace Carbon.Platform.Networking
{
    public interface INetworkInterface : IResource
    {
        MacAddress MacAddress { get; }

        IPAddress[] Addresses { get; }
    
        // NetworkId
        // SubnetId
    }
}

//       NAME
// GPC | compute#networkInterface


// Network interfaces may be attached to hosts, load balancers, db instances, etc.