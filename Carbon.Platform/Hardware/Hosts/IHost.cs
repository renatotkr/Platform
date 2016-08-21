namespace Carbon.Platform
{
    public interface IHost
    {
        long Id { get; }
    }
}

// HostIds

// Zones allocate a hostId...

// upper32 = zoneId
// lower32 = hostId

// Allocate each region 65,536 blocks for hosts
// Have zones ask for blocks from the region, as needed