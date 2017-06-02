using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.CI
{
    [Dataset("Deployments", Schema = "CI")]
    public class Deployment : IDeployment, IResource
    {
        public Deployment() { }

        public Deployment(
            long id,
            IRelease release,
            long initiatorId,
            DeploymentStatus status = DeploymentStatus.Pending)
        {
            #region Preconditions

            Validate.Id(id);

            Validate.NotNull(release, nameof(release));

            Validate.Id(initiatorId, nameof(initiatorId));

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
