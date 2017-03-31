namespace Carbon.Platform.Networking
{
    public interface INetworkInterfaceStats
    {
        long BytesReceived { get; }

        long BytesSent { get; }

        long PacketsReceived { get; }

        long PacketsSent { get; }

        long PacketsDiscarded { get; }
    }
}
