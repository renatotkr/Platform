using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public interface ILocation : IResource
    {
        // e.g. us-east-1, us-east-1a
        string Name { get; }
    }
}