namespace Carbon.CI
{
    public enum DeploymentStatus : byte
    {
        Pending   = 1,
        Running   = 2, 
        Succeeded = 3,
        Failed    = 4
    }
}