using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface IImage : IManagedResource
    {
        ImageType Type { get; }

        string Name { get; }
    }
}

// aws | ami-1a2b3c4d        | Image / Amazon Machine Image
// gcp | 6864121747226459524 | compute#image