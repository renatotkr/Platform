using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Web
{
    [Dataset("Websites")]
    public class WebsiteInfo : IWebsite
    {
        public WebsiteInfo() { }

        public WebsiteInfo(
            long id,
            string name, 
            long environmentId,
            long repositoryId, 
            long ownerId)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (environmentId <= 0)
                throw new ArgumentException("Must be > 0", nameof(environmentId));

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

            Id            = id;
            Name          = name ?? throw new ArgumentNullException(nameof(name));
            EnvironmentId = environmentId;
            RepositoryId  = repositoryId;
            OwnerId       = ownerId;
        }

        [Member("id"), Key(sequenceName: "websiteId")] 
        public long Id { get; }
        
        [Member("name"), Unique]
        [StringLength(63)]
        public string Name { get; }

        [IgnoreDataMember]
        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("environmentId")]
        public long EnvironmentId { get; }

        [Member("repositoryId")]
        public long RepositoryId { get; }

        [Member("deploymentId")]
        public long? DeploymentId { get; set; }
        
        #region Stats

        [Member("releaseCount")]
        public int ReleaseCount { get; }

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }
        
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion

        public override string ToString() => Name;
    }
}