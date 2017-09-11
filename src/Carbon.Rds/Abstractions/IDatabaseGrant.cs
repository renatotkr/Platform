namespace Carbon.Rds
{
    public interface IDatabaseGrant
    {
        long Id { get; }

        string SchemaName { get; }

        string ObjectName { get;  }
        
        string[] ColumnNames { get; }

        string[] Actions { get; }

        long UserId { get; }
    }
}