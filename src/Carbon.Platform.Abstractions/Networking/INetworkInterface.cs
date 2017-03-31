namespace Carbon.Platform.Networking
{
    public interface INetworkInterface
    {
        long Id { get; }

        string MacAddress { get; }

        int ProviderId { get; }
    }
}