namespace Carbon.Rds
{
    public interface IDatabaseMigration
    {
        long Id { get; }

        string SchemaName { get; }

        string Description { get; }

        string[] Commands { get; }
    }
}