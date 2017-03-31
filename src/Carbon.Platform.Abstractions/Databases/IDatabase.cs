namespace Carbon.Platform.Databases
{
    public interface IDatabase
    {
        long Id { get; }

        string Name { get; }
    }
}