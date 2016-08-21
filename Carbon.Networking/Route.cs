namespace Carbon.Networking
{
    public class Route
    {
        public long Id { get; set; }

        public long NetworkId { get; set; }

        public long NextHopId { get; set; } // IPAddress
    }
}
