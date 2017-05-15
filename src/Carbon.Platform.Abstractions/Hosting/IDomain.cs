using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    public interface IDomain : IResource
    {
        string Name { get; }
    }
}