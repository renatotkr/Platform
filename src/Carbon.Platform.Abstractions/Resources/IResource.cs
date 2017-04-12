namespace Carbon.Platform.Resources
{
    public interface IResource
    {
        long Id { get; }

        ResourceType ResourceType { get; }
    }
}