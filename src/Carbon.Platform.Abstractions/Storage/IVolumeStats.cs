namespace Carbon.Platform.Storage
{
    public interface IVolumeStats
    {
        long ReadBytes { get; }

        long WriteBytes { get; }
    }
}