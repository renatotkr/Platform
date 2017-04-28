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

    // The application's environment determines whether an application endpoint is exposed (i.e. a web app)
}