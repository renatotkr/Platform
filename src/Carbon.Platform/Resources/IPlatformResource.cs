namespace Carbon.Platform
{
    public interface ICloudResource
    {
        ResourceType Type { get; }

        CloudProvider Provider { get; }
    }
}