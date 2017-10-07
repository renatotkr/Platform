using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    public interface IDomain : IResource
    {
        // note: punycoded
        string Name { get; }
    }
}