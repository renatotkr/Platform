namespace Carbon.Platform
{
    public enum HostStatus
    {
        Pending     = 0, // provisioning
        Running     = 1,
        Suspending  = 2, // stopping
        Suspended   = 3, // stopped
        Terminating = 4, // shutting down ?
        Terminated  = 5 
    }

    // Staging?
}