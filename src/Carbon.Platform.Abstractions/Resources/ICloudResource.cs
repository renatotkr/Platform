namespace Carbon.Platform
{
    public interface ICloudResource
    {
        ResourceType Type { get; }

        string ResourceId { get; } // Identity assigned by provider within the scope of the type

        int    ProviderId { get; }
    }
}