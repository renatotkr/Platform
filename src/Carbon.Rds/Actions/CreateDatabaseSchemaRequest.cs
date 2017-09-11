using Carbon.Data.Expressions;

namespace Carbon.Rds.Services
{
    public class CreateDatabaseSchemaRequest
    {
        public long DatabaseId { get; set; }

        public string SchemaName { get; set; }
    }
}