namespace Carbon.Platform.Computing
{
    public enum HostStatus : byte
    {
                         // AWS             | Azure        | GPC           | DO
        Pending     = 1, // pending         | Starting     | PROVISIONING  | new
        Running     = 2, // running         | Running      | RUNNING       | active
        Stopping    = 3, // stopping        | Stopping     | STOPPING      | ?
        Stopped     = 4, // stopped         | Stopped      | STOPPED       | off
        Terminating = 5, // shutting-down   | Deallocating |               | ?
        Terminated  = 6  // terminated      | Deallocated  | TERMINATED    | archived
    }
}

// GPC also has a suspending & suspended state