﻿using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Databases
{
    [Dataset("DatabaseInstances")]
    public class DatabaseInstance : ICloudResource
    {
        [Member("id"), Key]
        public long Id { get; set; }
        
        // [Member("priority")]
        // public int Priority { get; set; }

        [Member("databaseId")]
        public long DatabaseId { get; set; }

        // MYSQL@5.5

        [Member("databaseType")] 
        public string DatabaseType { get; set; }

        [Member("status"), Mutable]
        public DatabaseStatus Status { get; set; }

        [Member("flags")]
        public DatabaseFlags Flags { get; set; }

        [Member("locationId")]
        public long LocationId { get; set; }

        [Member("heatbeat")]
        public DateTime? Heartbeat { get; }

        [Member("terminated")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Terminated { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }

        #region Helpers

        [IgnoreDataMember]
        public bool IsTerminated => Terminated != null;

        [IgnoreDataMember]
        public bool IsPrimary => Flags.HasFlag(DatabaseFlags.Primary);

        [IgnoreDataMember]
        public bool IsReadOnly => Flags.HasFlag(DatabaseFlags.ReadOnly);

        #endregion

        ResourceType ICloudResource.Type => ResourceType.Database;

        [IgnoreDataMember]
        CloudProvider ICloudResource.Provider => LocationHelper.GetProvider(LocationId);
    }
}