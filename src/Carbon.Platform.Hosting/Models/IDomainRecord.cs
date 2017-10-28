namespace Carbon.Platform.Hosting
{
    public interface IDomainRecord
    {
        long Id { get; }

        long DomainId { get; }

        string Name { get; }

        int? Ttl { get; }
    }
}