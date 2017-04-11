using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Apps
{
    [Dataset("Apps")]
    public class AppInfo : IApp
    {
        public AppInfo() { }

        public AppInfo(long id, string name, AppType type, long ownerId)
        {
            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
            OwnerId = ownerId;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("type")]
        public AppType Type { get; }
        
        [Member("name"), Unique]
        [StringLength(63)]
        public string Name { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}