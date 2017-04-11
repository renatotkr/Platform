namespace Carbon.Platform.Networking
{
    public interface INetworkProxyRule
    {
        long Id { get; }

        long NetworkProxyId { get; }

        string Condition { get; }

        string Action { get; }

        int Priority { get; }
    }
}


/*
path matches /images/*
host matches carbon.net
*/