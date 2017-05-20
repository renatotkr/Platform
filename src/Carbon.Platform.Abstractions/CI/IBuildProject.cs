using Carbon.Platform.Resources;

namespace Carbon.Platform.CI
{ 
    public interface IBuildProject : IResource
    {
        string Name { get; }

        long RepositoryId { get; }
    }
}