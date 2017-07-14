using Carbon.Platform.Resources;
using Carbon.Platform.Storage;

namespace Carbon.Rds
{
    public class RegisterDatabaseInstanceRequest
    {
        public RegisterDatabaseInstanceRequest() { }

        public RegisterDatabaseInstanceRequest(
            ManagedResource resource, 
            int priority = 1,
            DatabaseFlags flags = DatabaseFlags.Primary)
        {
            Resource = resource;
            Priority = priority;
            Flags    = flags;
        }

        public long DatabaseId { get; set; }

        public long ClusterId { get; set; }

        public ManagedResource Resource { get; set; }

        public int Priority { get; set; } = 1;

        public DatabaseFlags Flags { get; set; } = DatabaseFlags.Primary;
    }
}