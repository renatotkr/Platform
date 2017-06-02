using Carbon.Platform.Resources;

namespace Carbon.CI
{ 
    public interface IBuildProject : IResource
    {
        string Name { get; }

        long RepositoryId { get; }
    }
}