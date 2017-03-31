namespace Carbon.Platform.Networking
{
    public interface ISubnet
    {
        long Id { get; }

        long NetworkId { get; }

        string Cidr { get; }

        long LocationId { get; }
    }
}