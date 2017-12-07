namespace Carbon.Platform.Configuration.Docker
{
    public class Network
    {
        public NetworkMode Mode { get; set; }

        public string[] DnsServers { get; set; }

        public string MacAddress { get; set; }

        public string Container { get; set; }

        public bool AddHost { get; set; }

        /*
        --dns=[]         : Set custom dns servers for the container
        --net="bridge"   : Set the Network mode for the container
                            'bridge': creates a new network stack for the container on the docker bridge
                            'none': no networking for this container
                            'container:<name|id>': reuses another container network stack
                            'host': use the host network stack inside the container
        --add-host=""    : Add a line to /etc/hosts (host:IP)
        --mac-address="" : Sets the container's Ethernet device's MAC address
       */
    }

    public enum NetworkMode
    {
        Bridge    = 0, // default: Connect the container to the bridge via veth interfaces.
        None      = 1, // No networking in the container.
        Container = 2, // Use the host's network stack inside the container.
        Host      = 3  // Use the network stack of another container, specified via its *name* or *id*.
    }
}