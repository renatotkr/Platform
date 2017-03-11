namespace Carbon.Platform.Networking
{
    public enum NetworkLayer : byte
    {
        Physical     = 1,
        Datalink     = 2,
        Internet     = 3,
        EndToEnd     = 4,
        Applications = 7
    }
}