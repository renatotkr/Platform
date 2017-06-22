namespace Carbon.Platform.Computing
{
    public enum HostStatus : byte
    {
        Pending     = 1, // provisioning (include gcp's staging state)
        Running     = 2,
        Suspending  = 3, // stopping
        Suspended   = 4, // stopped
        Terminating = 5, // shutting down ?
        Terminated  = 6
    }
}