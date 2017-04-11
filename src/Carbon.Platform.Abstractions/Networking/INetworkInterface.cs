using System.Net;

using Carbon.Net;

namespace Carbon.Platform.Networking
{
    public interface INetworkInterface : IManagedResource
    {
        long Id { get; }

        MacAddress MacAddress { get; }

        IPAddress[] Addresses { get; }
    }
}