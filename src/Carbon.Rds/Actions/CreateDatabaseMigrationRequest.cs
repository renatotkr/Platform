using System;

namespace Carbon.Rds
{
    public class CreateDatabaseMigrationRequest
    {
        public CreateDatabaseMigrationRequest(
            long databaseId, 
            string schemaName, 
            string[] commands, 
            string description = null)
        {
            DatabaseId  = databaseId;
            SchemaName  = schemaName;
            Commands = commands ?? throw new ArgumentNullException(nameof(commands));
            Description = description;

        }
        public long DatabaseId { get; }

        public string SchemaName { get; }

        public string Description { get; }

        public string[] Commands { get; }
    }
}