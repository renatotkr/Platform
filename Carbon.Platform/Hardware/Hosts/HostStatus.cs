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
}

/* 
EC2
0  : pending
16 : running
32 : shutting-down
48 : terminated
64 : stopping
80 : stopped

GCE
Provisioning
Staging
Running
Stopping
Suspending
Suspended
Terminated
*/