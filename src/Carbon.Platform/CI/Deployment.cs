using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.CI
{
    [Dataset("Deployments")]
    public class Deployment : IDeployment
    {
        public Deployment() { }

        public Deployment(
            long id,
            IRelease release,
            long initiatorId,
            DeploymentStatus status = DeploymentStatus.Pending)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            if (initiatorId <= 0)
                throw new ArgumentException("Must be > 0", nameof(initiatorId));

            #endregion

            Id          = id;
            Status      = status;
            ReleaseType = release.Type;
            ReleaseId   = release.Id;
            InitiatorId = initiatorId;
        }

        // environmentId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("status")]
        public DeploymentStatus Status { get; set; }
       
        // Website | Program
        [Member("releaseType")]
        public ReleaseType ReleaseType { get; }

        [Member("releaseId")]
        public long ReleaseId { get; }

        [Member("initiatorId")]
        public long InitiatorId { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Deployment;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("completed")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Completed { get; set; }

        #endregion

        public long EnvironmentId => ScopedId.GetScope(Id);
    }
}
