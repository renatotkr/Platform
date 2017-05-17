namespace Carbon.Platform.Storage
{
    public interface IDatabaseEndpoint
    {
        string Host { get; }

        ushort Port { get; }

        bool IsReadOnly { get; }
    }
}