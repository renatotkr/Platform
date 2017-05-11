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

        public AppInfo(long id, string name, long ownerId)
        {
            Id      = id;
            Name    = name ?? throw new ArgumentNullException(nameof(name));
            OwnerId = ownerId;
        }

        [Member("id"), Key]
        public long Id { get; }
        
        [Member("name"), Unique]
        [StringLength(63)]
        public string Name { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        #region Stats

        [Member("releaseCount")]
        public int ReleaseCount { get; }

        #endregion

        #region Resource

        [Member("repositoryId")]
        public long RepositoryId { get; set; }

        #endregion

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

// An app spans four environments: development, intergration, staging, and production
// The app's environment is responsible for exposing itself (i.e. to the web) or staying internal