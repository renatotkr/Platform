using Carbon.Platform.Resources;

namespace Carbon.Platform.CI
{ 
    public interface IBuildProject : IManagedResource
    {
        string Name { get; }
    }
}