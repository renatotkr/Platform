using System;

namespace Carbon.Platform.Networking
{
    public interface INetworkInterfaceAttachment
    {
        long NetworkInterfaceId { get; }

        long HostId { get; }
        
        DateTime Created { get; }

        DateTime? Deleted { get; }
    }
}