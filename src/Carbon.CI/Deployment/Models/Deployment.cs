using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;
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
            DeploymentStatus status = DeploymentStatus.Pending,
            JsonObject properties = null)
        {
            #region Preconditions

            Validate.Id(id);

            if (release == null)
                throw new ArgumentNullException(nameof(release));
            
            Validate.Id(creatorId, nameof(creatorId));

            #endregion

            Id             = id;
            Status         = status;
            ProgramId      = release.ProgramId;
            ProgramVersion = release.Version;
            CreatorId      = creatorId;
            Properties     = properties;
        }

        [Member("id"), Key("deploymentId")]
        public long Id { get; }

        [Member("status")]
        public DeploymentStatus Status { get; set; }
       
        [Member("programId")]
        public long ProgramId { get; }

        [Member("programVersion")]
        public SemanticVersion ProgramVersion { get; }

        [Member("creatorId")]
        public long CreatorId { get; }

        // 1@1.1.4

        // packageName: 1/1.1.4.tar.gz

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Deployment;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("completed")]
        public DateTime? Completed { get; set; }

        #endregion
    }
}

// v2: all programs should be bundled into immutable images
