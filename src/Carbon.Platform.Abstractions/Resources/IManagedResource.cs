namespace Carbon.Platform
{
    public interface IManagedResource
    {
        int ProviderId { get; }

        long LocationId { get; }

        ResourceType ResourceType { get; }

        string ResourceId { get; } // Assigned by provider
    }
}