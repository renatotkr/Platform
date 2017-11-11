using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;
using Carbon.Versioning;

namespace Carbon.CI
{
    [Dataset("Deployments", Schema = CiadDb.Name)]
    public class Deployment : IDeployment, IResource
    {
        public Deployment() { }

        public Deployment(
            long id,
            long environmentId,
            IProgramRelease release,
            long creatorId,
            DeploymentStatus status = DeploymentStatus.Pending,
            JsonObject properties = null)
        {
            Validate.Id(id);
            Validate.Id(environmentId, nameof(environmentId));
            Validate.Id(creatorId, nameof(creatorId));

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            Id             = id;
            Status         = status;
            EnvironmentId  = environmentId;
            ProgramId      = release.ProgramId;
            ProgramVersion = release.Version;
            CreatorId      = creatorId;
            Properties     = properties;
        }
        
        [Member("id"), Key("deploymentId")]
        public long Id { get; }
        
        [Member("environmentId"), Indexed]
        public long EnvironmentId { get; }

        [Member("status"), Mutable]
        public DeploymentStatus Status { get; set; }
      
        [Member("programId")]
        public long ProgramId { get; }

        [Member("programVersion")]
        public SemanticVersion ProgramVersion { get; }

        [Member("creatorId")]
        public long CreatorId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Deployment;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("completed"), Mutable]
        public DateTime? Completed { get; set; }

        #endregion
    }
}