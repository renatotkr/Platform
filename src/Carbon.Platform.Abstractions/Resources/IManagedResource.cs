namespace Carbon.Platform.Resources
{
    public interface IManagedResource : IResource
    {
        int ProviderId { get; }

        long LocationId { get; }

        string ResourceId { get; } // Assigned by provider
    }
}