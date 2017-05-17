using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;
using Carbon.Versioning;

namespace Carbon.Platform.CI
{
    [Dataset("Deployments")]
    public class Deployment : IDeployment
    {
        public Deployment() { }

        public Deployment(
            long id,
            IRelease release,
            long creatorId,
            DeploymentStatus status = DeploymentStatus.Pending)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            if (creatorId <= 0)
                throw new ArgumentException("Must be > 0", nameof(creatorId));

            #endregion

            Id = id;
            Status         = status;
            ReleaseType    = release.Type;
            ReleaseId      = release.Id;
            ReleaseVersion = release.Version; 
            CommitId       = release.CommitId;
            CreatorId      = creatorId; // may be different from the releaser
        }

        // environmentId + deployCount [within enviornment]
        [Member("id"), Key]
        public long Id { get; }

        [Member("status")]
        public DeploymentStatus Status { get; set; }
       
        // Website | Program
        [Member("releaseType")]
        public ReleaseType ReleaseType { get; }

        [Member("resourceId")]
        public long ReleaseId { get; }

        [Member("releaseVersion")]
        public SemanticVersion ReleaseVersion { get; }

        [Member("commitId")]
        public long CommitId { get; }

        [Member("creatorId")]
        public long CreatorId { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceType.Deployment;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("completed")]
        public DateTime? Completed { get; set; }

        #endregion

        public long EnvironmentId => ScopedId.GetScope(Id);
    }
}
