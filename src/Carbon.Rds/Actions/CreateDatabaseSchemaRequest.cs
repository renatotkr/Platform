using System.Runtime.Serialization;

namespace Carbon.Rds.Services
{
    public class CreateDatabaseSchemaRequest
    {
        public CreateDatabaseSchemaRequest() { }

        public CreateDatabaseSchemaRequest(long databaseId, string schemeName)
        {
            DatabaseId = databaseId;
            SchemaName = schemeName;
        }

        [DataMember(Name = "databaseId")]
        public long DatabaseId { get; set; }

        [DataMember(Name = "schemaName")]
        public string SchemaName { get; set; }
    }
}