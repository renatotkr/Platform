using System;
using Carbon.Data.Annotations;

namespace Carbon.CI
{
    [Dataset("DeploymentTargets", Schema = CiadDb.Name)]
    public class DeploymentTarget : IDeploymentTarget
    {
        public DeploymentTarget() { }

        public DeploymentTarget(
            long deploymentId, 
            long hostId,
            DeploymentStatus status = DeploymentStatus.Pending,
            string message = null)
        {
            #region Preconditions

            Validate.Id(deploymentId, nameof(deploymentId));

            Validate.Id(hostId, nameof(hostId));
            
            #endregion

            DeploymentId = deploymentId;
            HostId       = hostId;
            Status       = status;
            Message      = message;
        }

        [Member("deploymentId"), Key]
        public long DeploymentId { get; }

        [Member("hostId"), Key]
        public long HostId { get; }

        [Member("status")]
        public DeploymentStatus Status { get; set; }
        
        [Member("message")]
        public string Message { get; set; }
    }
}
