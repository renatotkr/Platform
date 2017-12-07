namespace Carbon.Rds.Services
{
    public class RegisterDatabaseRequest
    {
        public RegisterDatabaseRequest(
            string name, 
            long ownerId,
            string[] schemaNames = null,
            RegisterDatabaseClusterRequest[] clusters = null)
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.Id(ownerId, nameof(ownerId));

            Name        = name;
            OwnerId     = ownerId;
            SchemaNames = schemaNames;
            Clusters    = clusters;
        }

        public long OwnerId { get; }

        public string Name { get; }
        
        public string[] SchemaNames { get; }

        public RegisterDatabaseClusterRequest[] Clusters { get; }
    }
}