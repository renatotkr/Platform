namespace Carbon.Platform.Networking
{
    public interface INetworkProxyRule
    {
        long Id { get; }

        long ProxyId { get; }

        string Condition { get; }
        
        string Action { get; }
        
        int Priority { get; }
    }
}