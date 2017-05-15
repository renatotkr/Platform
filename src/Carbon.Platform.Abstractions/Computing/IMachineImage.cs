using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface IMachineImage : IManagedResource
    {
        MachineImageType Type { get; }

        // Size?

        string Description { get; }
    }
}

// aws | ami-1a2b3c4d        | Image / Amazon Machine Image
// gcp | 6864121747226459524 | compute#image