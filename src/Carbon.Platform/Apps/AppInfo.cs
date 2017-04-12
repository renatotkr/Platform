using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Apps
{
    [Dataset("Apps")]
    public class AppInfo : IApp, IResource
    {
        public AppInfo() { }

        public AppInfo(long id, string name, AppType type)
        {
            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("type")]
        public AppType Type { get; }
        
        [Member("name"), Unique]
        [StringLength(63)]
        public string Name { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceType.App;

        #endregion

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