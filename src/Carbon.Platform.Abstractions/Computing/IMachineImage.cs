using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface IMachineImage : IManagedResource
    {
        MachineImageType Type { get; }

        string Description { get; }
    }
}

//  Google: diskSizeGb

// aws    : ami-1a2b3c4d                 | Image
// google : 6864121747226459524          | compute#image
