namespace Carbon.Platform.Storage
{
    public interface IVolume
    {
        long Id { get; }

        long Size { get; }

        long LocationId { get; }
    }
}