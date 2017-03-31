namespace Carbon.Platform.Computing
{
    public interface IMachineImage
    {
        long Id { get; }

        ImageType Type { get; }

        int ProviderId { get; }
    }
}