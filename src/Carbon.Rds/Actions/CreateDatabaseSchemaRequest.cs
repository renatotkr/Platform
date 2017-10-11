using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Carbon.Rds.Services
{
    public class CreateDatabaseSchemaRequest
    {
        public CreateDatabaseSchemaRequest() { }

        public CreateDatabaseSchemaRequest(long databaseId, string schemeName)
        {
            DatabaseId = databaseId;
            SchemaName = schemeName ?? throw new ArgumentNullException(nameof(schemeName));
        }

        [DataMember(Name = "databaseId")]
        public long DatabaseId { get; set; }

        [DataMember(Name = "schemaName")]
        [StringLength(63)]
        public string SchemaName { get; set; }
    }
}