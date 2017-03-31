using System;

namespace Carbon.Platform
{
    public interface INetworkInterfaceAttachment
    {
        long NetworkInterfaceId { get; }

        long HostId { get; }
        
        DateTime Created { get; }

        DateTime? Deleted { get; }
    }
}