using System;

namespace Carbon.Platform.Computing
{
    public interface IVolumeAttachment
    {
        long VolumeId { get; }

        long HostId { get; }
        
        DateTime Created { get; }

        DateTime? Deleted { get; }
    }
}