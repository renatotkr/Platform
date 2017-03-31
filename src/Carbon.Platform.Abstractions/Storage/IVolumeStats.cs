namespace Carbon.Platform.Storage
{
    public interface IVolumeStats
    {
        long BytesRead { get; }

        long BytesWritten { get; }
    }
}