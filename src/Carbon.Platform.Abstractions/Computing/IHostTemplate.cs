﻿using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface IHostTemplate : IManagedResource
    {
        string Name { get; }

        long MachineImageId { get; }

        long MachineTypeId { get; }

        // Script run at startup
        string StartupScript { get; }
    }
}

// aws | instance launch configuration
// gcp | instance templates
