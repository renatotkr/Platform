using System;
using Carbon.Data.Annotations;

namespace Carbon.CI
{
    [Dataset("DeploymentTargets")]
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

            if (deploymentId <= 0)
                throw new ArgumentException("Must be > 0", nameof(deploymentId));

            if (hostId <= 0)
                throw new ArgumentException("Must be > 0", nameof(hostId));

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

        // ClusterId?

        [Member("status")]
        public DeploymentStatus Status { get; }
        
        [Member("message")]
        public string Message { get; }
    }
}
