using System;

using Carbon.Data.Annotations;
using Carbon.Json;
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
            long creatorId,
            DeploymentStatus status = DeploymentStatus.Pending)
        {
            #region Preconditions

            Validate.Id(id);

            Validate.NotNull(release, nameof(release));
            
            Validate.Id(creatorId, nameof(creatorId));

            #endregion

            Id             = id;
            Status         = status;
            ReleaseId      = release.Id;
            ProgramId      = release.ProgramId;
            ProgramVersion = release.Version;
            CreatorId      = creatorId;
        }

        // environmentId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("status")]
        public DeploymentStatus Status { get; set; }
       
        [Member("releaseId")]
        public long ReleaseId { get; }
        
        [Member("programId")]
        public long ProgramId { get; }

        [Member("programVersion")]
        public SemanticVersion ProgramVersion { get; }

        [Member("creatorId")]
        public long CreatorId { get; }

        // packageName: 1/1.1.4.tar.gz

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; set; }

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
