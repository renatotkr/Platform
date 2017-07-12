using System;
using System.Runtime.Serialization;
using Carbon.Data.Annotations;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Storage
{
    [Dataset("DatabaseInstances")]
    public class DatabaseInstance : IDatabaseInstance
    {
        public DatabaseInstance() { }

        public DatabaseInstance(
            long id,
            DatabaseFlags flags, 
            long? clusterId,
            int priority,
            ManagedResource resource)
        {
            Id        = id;
            Flags     = flags;
            ClusterId = clusterId;
            Priority  = priority;

            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
        }

        // databaseId | #
        [Member("id"), Key]
        public long Id { get; }
        
        [Member("status")]
        public DatabaseStatus Status { get; set; }

        [Member("flags")]
        public DatabaseFlags Flags { get; }

        [Member("clusterId")]
        public long? ClusterId { get; }

        [Member("priority")]
        public int Priority { get; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [StringLength(100)]
        public string ResourceId { get; }

        [IgnoreDataMember]
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

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion

        #region Helpers

        [IgnoreDataMember]
        public bool IsTerminated => Terminated != null;

        [IgnoreDataMember]
        public bool IsPrimary => Flags.HasFlag(DatabaseFlags.Primary);

        [IgnoreDataMember]
        public bool IsReadOnly => Flags.HasFlag(DatabaseFlags.ReadOnly);

        #endregion

        public long DatabaseId => ScopedId.GetScope(Id);
    }
}
