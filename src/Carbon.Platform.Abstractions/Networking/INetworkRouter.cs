namespace Carbon.Platform.Networking
{
    public interface INetworkRouter
    {
        long Id { get; }

        long NetworkId { get; }

        long LocationId { get; }

        int ProviderId { get; }
    }
}

// Google: compute#router