namespace Carbon.CI
{
    public interface IDeploymentTarget
    {
       long DeploymentId { get; }

       long HostId { get; }
    }
}
