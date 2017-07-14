namespace Carbon.Rds
{
    public interface IDatabaseEndpoint
    {
        long DatabaseId { get; }

        string Host { get; }

        ushort Port { get; }

        bool IsReadOnly { get; }
    }
}