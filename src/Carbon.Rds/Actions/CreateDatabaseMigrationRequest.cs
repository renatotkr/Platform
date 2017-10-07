using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Carbon.Rds.Services
{
    public class CreateDatabaseMigrationRequest
    {
        public CreateDatabaseMigrationRequest() { }

        public CreateDatabaseMigrationRequest(
            long databaseId, 
            string schemaName, 
            string[] commands, 
            string description = null)
        {
            DatabaseId  = databaseId;
            SchemaName  = schemaName;
            Commands    = commands ?? throw new ArgumentNullException(nameof(commands));
            Description = description;
        }

        [DataMember(Name = "databaseId")]
        public long DatabaseId { get; set; }

        [DataMember(Name = "schemaName")]
        public string SchemaName { get; set; }

        [DataMember(Name = "commands"), Required]
        public string[] Commands { get; set;  }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}