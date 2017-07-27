﻿namespace Carbon.Platform.Computing
{
    public enum ProgramType : byte
    {
        App     = 1,
        Service = 2,
        Site    = 3,
        Task    = 4,
        Library = 5
    }
    
    // /var/apps
    // /var/sites
    // /var/tasks
}