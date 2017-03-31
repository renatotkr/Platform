namespace Carbon.Platform.Networking
{
    public interface INetwork
    {
        long Id { get; }

        int? ASN { get; }

        string Cidr { get; }
        
        int ProviderId { get; }
    }
}