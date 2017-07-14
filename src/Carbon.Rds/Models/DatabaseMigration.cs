using System;
using Carbon.Data.Annotations;

namespace Carbon.Rds
{
    [Dataset("DatabaseMigrations", Schema = "Rds")]
    public class DatabaseMigration : IDatabaseMigration
    {
        public DatabaseMigration() { }

        public DatabaseMigration(long id, string schemaName, string[] commands, string description = null)
        {
            Id          = id;
            SchemaName  = schemaName;
            Commands    = commands;
            Description = description;
        }

        // databaseId | #
        [Member("id"), Key]
        public long Id { get; }
        
        [Member("schemaName")]
        [StringLength(100)]
        public string SchemaName { get; }

        [Member("description")]
        [StringLength(2000)]
        public string Description { get; }

        [Member("commands")]
        [StringLength(10000)]
        public string[] Commands { get; }

        [Member("status")]
        public DatabaseMigrationStatus Status { get; }
        
        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion
    }

    public enum DatabaseMigrationStatus
    {
        Success = 1,
        Failed  = 2
    }

}

// https://en.wikipedia.org/wiki/Schema_migration