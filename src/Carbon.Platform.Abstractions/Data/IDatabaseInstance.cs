namespace Carbon.Platform.Data
{
    public interface IDatabaseInstance : IManagedResource
    {
        long Id { get; }

        long DatabaseId { get; }
    }
}