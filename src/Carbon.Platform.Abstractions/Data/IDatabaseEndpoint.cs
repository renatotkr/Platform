namespace Carbon.Platform.Data
{
    public interface IDatabaseEndpoint
    {
        string Host { get; }

        ushort Port { get; }

        bool IsReadOnly { get; }
    }
}