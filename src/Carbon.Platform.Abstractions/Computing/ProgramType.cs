namespace Carbon.Platform.Computing
{
    public enum ProgramType : byte
    {
        Application = 1, // user interactive
        Service     = 2,
        Task        = 3
    }

    // The operating system is above this...
}

// /var/apps/accelerator
// /var/services/accelerator
// /var/tasks/something

// https://en.wikipedia.org/wiki/Computer_program

// ProgramType (Application, Service, ...)