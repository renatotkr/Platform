using System;

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
            Name        = name ?? throw new ArgumentNullException(nameof(name));
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