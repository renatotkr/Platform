using Carbon.Data.Annotations;

namespace Carbon.Platform.CI
{
    [Dataset("DeploymentTargets")]
    public class DeploymentTarget : IDeploymentTarget
    {
        public DeploymentTarget(
            long deploymentId, 
            long hostId,
            DeploymentStatus status = DeploymentStatus.Pending,
            string message = null)
        {
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
        public DeploymentStatus Status { get; }
        
        [Member("message")]
        public string Message { get; }
    }
}
