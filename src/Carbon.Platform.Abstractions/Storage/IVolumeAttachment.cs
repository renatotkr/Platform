using System;

namespace Carbon.Platform
{
    public interface IVolumeAttachment
    {
        long VolumeId { get; }

        long HostId { get; }
        
        DateTime Created { get; }

        DateTime? Deleted { get; }
    }
}