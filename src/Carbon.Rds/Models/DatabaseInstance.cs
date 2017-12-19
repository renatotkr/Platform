using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;
using Carbon.Platform.Storage;

namespace Carbon.Rds
{
    [Dataset("DatabaseInstances", Schema = "Rds")]
    public class DatabaseInstance : IDatabaseInstance
    {
        public DatabaseInstance() { }

        public DatabaseInstance(
            long id,
            long clusterId,
            DatabaseFlags flags, 
            int priority,
            ManagedResource resource)
        {
            Id         = id;
            Flags      = flags;
            ClusterId  = clusterId;
            Priority   = priority;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
        }

        // databaseId | #
        [Member("id"), Key]
        public long Id { get; }
      
        [Member("hostId")]
        public long HostId { get; }

        [Member("clusterId")]
        public long ClusterId { get; }

        [Member("priority")]
        public int Priority { get; }

        [Member("status")]
        public HostStatus Status { get; set; }

        [Member("flags")]
        public DatabaseFlags Flags { get; }

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("resourceId")]
        [StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.DatabaseInstance;

        #endregion

        #region Timestamps

        [Member("heatbeat")]
        public DateTime? Heartbeat { get; }

        [Member("terminated")]
        public DateTime? Terminated { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion

        #region Helpers

        [IgnoreDataMember]
        public bool IsTerminated => Terminated != null;

        [IgnoreDataMember]
        public bool IsPrimary => (Flags & DatabaseFlags.Primary) != 0;

        [IgnoreDataMember]
        public bool IsReadOnly => (Flags & DatabaseFlags.ReadOnly) != 0;

        #endregion

        public long DatabaseId => ScopedId.GetScope(Id);
    }
}
