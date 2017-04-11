namespace Carbon.Platform.Computing
{
    public interface IVolumeStats
    {
        long ReadBytes { get; }

        long WriteBytes { get; }
    }
}