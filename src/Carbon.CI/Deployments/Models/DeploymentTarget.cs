using System;

using Carbon.Data.Annotations;

namespace Carbon.CI
{
    [Dataset("DeploymentTargets", Schema = CiadDb.Name)]
    public class DeploymentTarget
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

        [Member("status")]
        public DeploymentStatus Status { get; set; }
        
        [Member("message"), Optional]
        [StringLength(255)]
        public string Message { get; set; }
    }
}
