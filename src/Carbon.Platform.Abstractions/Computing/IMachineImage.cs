using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface IImage : IResource
    {
        ImageType Type { get; }

        string Name { get; }
    }
}

// postgres@9.3 746b819f315e  4 days ago 213.4 MB
// java@8       308e519aac60  6 days ago 824.5 MB

// provider: DockerHub...

// aws | ami-1a2b3c4d        | Image / Amazon Machine Image
// gcp | 6864121747226459524 | compute#image