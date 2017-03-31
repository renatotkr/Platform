namespace Carbon.Platform.Networking
{
    public interface INetworkProxyRule : INetworkRule
    {
        long Id { get; }

        long ProxyId { get; }
    }
}