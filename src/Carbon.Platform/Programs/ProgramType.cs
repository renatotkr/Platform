namespace Carbon.Platform.Computing
{
    public enum ProgramType
    {
        Unknown  = 0,
        Firmware = 1,
        OS       = 2,
        Task     = 3, // terminates after completion
        Service  = 4, // e.g. Webapp / Worker
    }
}