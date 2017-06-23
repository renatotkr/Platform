using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;
using Carbon.Versioning;

namespace Carbon.CI
{
    [Dataset("Deployments", Schema = "Ciad")]
    public class Deployment : IDeployment, IResource
    {
        public Deployment() { }

        public Deployment(
            long id,
            IProgramRelease release,
            long initiatorId,
            DeploymentStatus status = DeploymentStatus.Pending)
        {
            #region Preconditions

            Validate.Id(id);

            Validate.NotNull(release, nameof(release));
            
            Validate.Id(initiatorId, nameof(initiatorId));

            #endregion

            Id             = id;
            Status         = status;
            ReleaseId      = release.Id;
            ProgramId      = release.ProgramId;
            ProgramVersion = release.Version;
            InitiatorId    = initiatorId;
        }

        // environmentId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("status")]
        public DeploymentStatus Status { get; set; }
       
        // borg:program/1@1.234

        [Member("releaseId")]
        public long ReleaseId { get; }

        [Member("programId")]
        public long ProgramId { get; }

        [Member("programVersion")]
        public SemanticVersion ProgramVersion { get; }

        [Member("initiatorId")]
        public long InitiatorId { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Deployment;

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
