using System;

using Carbon.Platform.Resources;
using Carbon.Platform.Storage;

namespace Carbon.Rds.Services
{
    public class RegisterDatabaseInstanceRequest
    {
        public RegisterDatabaseInstanceRequest() { }

        public RegisterDatabaseInstanceRequest(
            long databaseId,
            ManagedResource resource, 
            int priority = 1,
            DatabaseFlags flags = DatabaseFlags.Primary)
        {
            #region Preconditions

            if (databaseId <= 0)
                throw new ArgumentException("Must be > 0", nameof(databaseId));

            #endregion

            DatabaseId = databaseId;
            Resource   = resource;
            Priority   = priority;
            Flags      = flags;
        }

        public long DatabaseId { get; set; }

        public long ClusterId { get; set; }

        public ManagedResource Resource { get; set; }

        public int Priority { get; set; }

        public DatabaseFlags Flags { get; set; }
    }
}