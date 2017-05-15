namespace Carbon.Platform.Computing
{
    public enum HostType : byte
    {
        Physical  = 1, // Metal         | Host
        Virtual   = 2, // Virtualized   | Instance
        Container = 3, // Containerized | ContainerInstance
    }
}