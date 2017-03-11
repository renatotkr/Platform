namespace Carbon.Platform.Computing
{
    public enum HostType : byte
    {
        Physical  = 1, // Metal
        Virtual   = 2, // Virtualized
        Container = 3, // Containerized
    }
}