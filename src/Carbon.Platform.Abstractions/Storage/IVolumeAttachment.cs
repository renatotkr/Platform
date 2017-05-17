using System;

namespace Carbon.Platform.Storage
{
    public interface IVolumeAttachment
    {
        long VolumeId { get; }

        long HostId { get; }
        
        DateTime Created { get; }

        DateTime? Deleted { get; }
    }
}