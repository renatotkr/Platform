namespace Carbon.Computing
{
    public enum ProgramType
    {
        Firmware = 1,
        OS       = 2,
        Task     = 3, // terminates after completion
        Service  = 4, // e.g. App
    }
}