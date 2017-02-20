namespace Carbon.Platform
{
    public interface ICloudResource
    {
        ResourceType Type { get; }

        CloudPlatformProvider Provider { get; }
    }
}