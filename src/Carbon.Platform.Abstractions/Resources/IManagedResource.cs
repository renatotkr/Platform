namespace Carbon.Platform
{
    public interface IManagedResource
    {
        ResourceType Type { get; }

        int ProviderId { get; }

        // Assigned by provider
        string ResourceId { get; }
    }
}