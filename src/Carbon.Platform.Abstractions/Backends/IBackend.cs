namespace Carbon.Platform.Computing
{
    public interface IBackend
    {
        long Id { get; }

        string Name { get; }

        int ProviderId { get; }
    }
}