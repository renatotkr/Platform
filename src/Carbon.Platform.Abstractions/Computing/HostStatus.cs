namespace Carbon.Platform.Computing
{
    public enum HostStatus : byte
    {
        Pending     = 0, // provisioning (include gcp's staging state)
        Running     = 1,
        Suspending  = 2, // stopping
        Suspended   = 3, // stopped
        Terminating = 4, // shutting down ?
        Terminated  = 5
    }
}