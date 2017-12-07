using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Carbon.Rds.Services
{
    public class CreateDatabaseSchemaRequest
    {
        public CreateDatabaseSchemaRequest(long databaseId, string schemeName)
        {
            Validate.Id(databaseId, nameof(databaseId));
            Validate.NotNullOrEmpty(schemeName, nameof(schemeName));

            DatabaseId = databaseId;
            SchemaName = schemeName;
        }

        [DataMember(Name = "databaseId")]
        public long DatabaseId { get; }

        [DataMember(Name = "schemaName")]
        [StringLength(63)]
        public string SchemaName { get; }
    }
}